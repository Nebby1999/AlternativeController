using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Nebula
{
    public abstract class ApplicationBehaviour<T> : MonoBehaviour where T : ApplicationBehaviour<T>
    {
        public static T instance { get; protected set; }
        public static bool loadStarted { get; private set; } = false;

        public static event Action OnLoad;
        public static event Action OnStart;
        public static event Action OnUpdate;
        public static event Action OnFixedUpdate;
        public static event Action OnLateUpdate;
        public static event Action OnShutdown;

        [SerializeField] private AssetReferenceScene _gameLoadingSceneName;
        [SerializeField] private AssetReferenceScene _loadingFinishedScene;
        [SerializeField] private AssetReferenceScene _inbetweenScenesLoadingScene;

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

        protected virtual IEnumerator C_LoadGame()
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

            //Special loading logic should happen only on runtime, so we're ommiting this when loading from the editor.
            //By ommiting this, we can load any scene and theoretically have entity states and the like running properly.
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;
            var address = sceneName + ".unity";
#endif

            var asyncOp = _gameLoadingSceneName.LoadSceneAsync();
            while (!asyncOp.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }

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

            //Special loading logic should happen only on runtime, so we're ommiting this when loading from the editor.
            //By ommiting this, we can load any scene and theoretically have entity states and the like running properly.
#if UNITY_EDITOR
            asyncOp = Addressables.LoadSceneAsync(address);
            while (!asyncOp.IsDone)
                yield return new WaitForEndOfFrame();
#else
            asyncOp = _loadingFinishedScene.LoadSceneAsync();
            while(!asyncOp.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }
#endif
        }

        protected abstract IEnumerator C_LoadGameContent();

        protected virtual void Start()
        {
            OnStart?.Invoke();
        }

        protected virtual void Update()
        {
            OnUpdate?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        protected virtual void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
        protected virtual void OnApplicationQuit()
        {
            OnShutdown?.Invoke();
        }
    }
}