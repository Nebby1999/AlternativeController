using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Nebula
{
    /// <summary>
    /// Un <see cref="ApplicationBehaviour{T}"/> es un monobehaviour especial que indica la existencia y instancia actual del videojuego.
    /// 
    /// <para>Tiene varias utilidades, como un sistema de carga a una escena en especifica, carga de contenido asincronico y eventos estacicos para correr metodos.</para>
    /// </summary>
    /// <typeparam name="T">El tipo de monobehaviour</typeparam>
    public abstract class ApplicationBehaviour<T> : MonoBehaviour where T : ApplicationBehaviour<T>
    {
        /// <summary>
        /// La instancia actual de la aplicacion
        /// </summary>
        public static T instance { get; protected set; }

        /// <summary>
        /// Verdadero si ya se inicio la carga de contenido del juego
        /// </summary>
        public static bool loadStarted { get; private set; } = false;

        /// <summary>
        /// Evento llamado cuando el contenido del juego acabo de cargarse.
        /// </summary>
        public static event Action OnLoad;

        /// <summary>
        /// Evento llamado cuando el Start de la aplicacion empieza.
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// Evento llamado en Update de la applicacion
        /// </summary>
        public static event Action OnUpdate;

        /// <summary>
        /// Evento llamado en FixedUpdate de la applicacion
        /// </summary>
        public static event Action OnFixedUpdate;

        /// <summary>
        /// Evento llamado en LateUpdate de la applicacion
        /// </summary>
        public static event Action OnLateUpdate;

        /// <summary>
        /// Evento llamado cuando la applicacion desea cerrarse.
        /// </summary>
        public static event Action OnShutdown;

        [Tooltip("Cuando el juego inicia por primera vez, iremos a esta escena para cargar el contenido del juego.")]
        [SerializeField] private string _loadingSceneName;
#if ADDRESSABLES
        [Tooltip("Cuando el juego termina de iniciarze, deberiamos ir a esta escena.")]
        [SerializeField] private AssetReferenceScene _loadingFinishedScene;
#endif

        /// <summary>
        /// Metodo AWake de la applicacion, inicializa el componente y empieza la carga de contenido del juego
        /// </summary>
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if(instance)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
            if(!loadStarted)
            {
                loadStarted = true;
                StartCoroutine(C_LoadGame());
            }
        }

        private IEnumerator C_LoadGame()
        {
            SceneManager.sceneLoaded += (s, e) =>
            {
                Debug.Log($"Loaded Scene {s.name} loadSceneMode={e}");
            };
            SceneManager.sceneUnloaded += (s) =>
            {
                Debug.Log($"Unloaded Scene {s.name}");
            };
            SceneManager.activeSceneChanged += (os, ns) =>
            {
                Debug.Log($"Active scene changed from {os.name} to {ns.name}");
            };

            //Logica especial de carga (IE, ir desde la escena de carga _loadingFinished) solo deberia ocurrir en runtime. Cuando cargamos en juego en el editor, omitiremos esto. De esta manera, cualquier escena que cargemos deberia tener las maquinas de estado funcionando correctamente.
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;
            var address = sceneName + ".unity";
#endif

            //Dentro del editor, cargemos la escena de carga de inmediato.
#if UNITY_EDITOR
            var asyncOp0 = SceneManager.LoadSceneAsync(_loadingSceneName);
            while (!asyncOp0.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            //Fuera del editor, esperamos a que la escena principal sea cargada.
#else
            while(SceneManager.GetActiveScene().name != _loadingSceneName)
            {
                yield return new WaitForEndOfFrame();
            }
#endif

            var beforeLoadingCoroutine = C_BeforeLoadingContent();
            while (!beforeLoadingCoroutine.MoveNext())
                yield return new WaitForEndOfFrame();

            yield return new WaitForEndOfFrame();

            IEnumerator contentLoadingCoroutine = C_LoadGameContent();
            while (contentLoadingCoroutine.MoveNext())
                yield return new WaitForEndOfFrame();

            yield return new WaitForEndOfFrame();

            if(OnLoad != null)
            {
                OnLoad();
                OnLoad = null;
            }

            IEnumerator finishingCoroutine = C_OnFinishedLoading();
            while (finishingCoroutine.MoveNext())
                yield return new WaitForEndOfFrame();

            //Logica especial de carga (IE, ir desde la escena de carga _loadingFinished) solo deberia ocurrir en runtime. Cuando cargamos en juego en el editor, omitiremos esto. De esta manera, cualquier escena que cargemos deberia tener las maquinas de estado funcionando correctamente.
#if UNITY_EDITOR
            var asyncOp1 = Addressables.LoadSceneAsync(address);
            while (!asyncOp1.IsDone)
                yield return new WaitForEndOfFrame();
#else
            var asyncOp1 = _loadingFinishedScene.LoadSceneAsync();
            while(!asyncOp1.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }
#endif
        }

        /// <summary>
        /// Coroutina llamada justo antes que <see cref="C_LoadGameContent"/> se llame.
        /// </summary>
        protected abstract IEnumerator C_BeforeLoadingContent();

        /// <summary>
        /// Coroutina que deberia cargar los assets del juego e inicializarlos si es necesario.
        /// </summary>
        protected abstract IEnumerator C_LoadGameContent();

        /// <summary>
        /// Coroutina que corre despues de <see cref="C_LoadGameContent"/>
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator C_OnFinishedLoading();

        /// <summary>
        /// Invoca <see cref="OnStart"/>
        /// </summary>
        protected virtual void Start()
        {
            OnStart?.Invoke();
        }

        /// <summary>
        /// Invoca <see cref="OnUpdate"/>
        /// </summary>
        protected virtual void Update()
        {
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Invoca <see cref="OnFixedUpdate"/>
        /// </summary>
        protected virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        /// <summary>
        /// Invoca <see cref="OnLateUpdate"/>
        /// </summary>
        protected virtual void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        /// <summary>
        /// Invoca <see cref="OnShutdown"/>
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            OnShutdown?.Invoke();
        }
    }
}