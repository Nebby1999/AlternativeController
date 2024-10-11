using Nebula;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Un <see cref="State"/> es una clase abstracta la cual representa un estado de una entidad, los estados se pueden heredar y se pueden ocupar para crear acciones complejas de un objeto sin necesidad de especificar toda la informacion dentro de un componente monolitico.
    /// <br></br>
    /// Los estados son ejecutados por <see cref="StateMachine"/>
    /// <para></para>
    /// Se debe heredar de esta clase para poder utilizar estados.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// El nombre de este estado
        /// </summary>
        public readonly string stateName;

        /// <summary>
        /// El nombre completo de este estado
        /// </summary>
        public readonly string fullStateName;

        /// <summary>
        /// El <see cref="StateMachine"/> que esta ejecutando este estado
        /// </summary>
        public StateMachine outer;

        /// <summary>
        /// El <see cref="GameObject"/> que es dueño de <see cref="outer"/>
        /// </summary>
        public GameObject gameObject => outer.gameObject;

        /// <summary>
        /// El transform acoplado a <see cref="gameObject"/>
        /// </summary>
        public Transform transform => outer.transform;

        /// <summary>
        /// La edad de este estado usando <see cref="Time.fixedDeltaTime"/>, representa por cuanto tiempo este estado a existido
        /// </summary>
        public float fixedAge { get; protected set; }

        /// <summary>
        /// La edad de este estado usando <see cref="Time.deltaTime"/>, representa por cuanto tiempo este estado a existido
        /// </summary>
        public float age { get; protected set; }

        /// <summary>
        /// Metodo que es llamado cuando el estado comienza.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Metodo que es llamado cuando el estado termina
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// Metodo equitativo a usar FixedUpdate dentro de un componente. aumenta <see cref="fixedAge"/> por <see cref="Time.fixedDeltaTime"/>
        /// </summary>
        public virtual void FixedUpdate()
        {
            fixedAge += Time.fixedDeltaTime;
        }

        /// <summary>
        /// Metodo equitativo a usar Update dentro de un componente. aumenta <see cref="age"/> por <see cref="Time.deltaTime"/>
        /// </summary>
        public virtual void Update()
        {
            age += Time.deltaTime;
        }

        /// <summary>
        /// Metodo que se puede sobre-escribir para modificar un nuevo estado especificado en <paramref name="nextState"/>
        /// </summary>
        /// <param name="nextState">El estado a modificar</param>
        public virtual void ModifyNextState(State nextState) { }

        /// <summary>
        /// Equitativo a usar <see cref="UnityEngine.Object.Destroy(UnityEngine.Object)"/>
        /// </summary>
        protected void Destroy(UnityEngine.Object obj) => UnityEngine.Object.Destroy(obj);

        /// <summary>
        /// Equitativo a usar <see cref="UnityEngine.Object.Instantiate(UnityEngine.Object)"/>
        /// </summary>
        protected T Instantiate<T>(T obj) where T : UnityEngine.Object => UnityEngine.Object.Instantiate(obj);

        /// <summary>
        /// Equitativo a usar <see cref="UnityEngine.Object.Instantiate(UnityEngine.Object, Transform)"/>
        /// </summary>
        protected T Instantiate<T>(T obj, Transform parent) where T : UnityEngine.Object => UnityEngine.Object.Instantiate(obj, parent);

        /// <summary>
        /// Equitativo a usar <see cref="UnityEngine.Object.Instantiate(UnityEngine.Object, Vector3, Quaternion)"/>
        /// </summary>
        protected T Instantiate<T>(T obj, Vector3 position, Quaternion rotation) where T : UnityEngine.Object => UnityEngine.Object.Instantiate(obj, position, rotation);

        /// <summary>
        /// Equitativo a usar <see cref="UnityEngine.Object.Instantiate(UnityEngine.Object, Vector3, Quaternion, Transform)"/>
        /// </summary>
        protected T Instantiate<T>(T obj, Vector3 position, Quaternion rotation, Transform parent) where T : UnityEngine.Object => UnityEngine.Object.Instantiate(obj, position, rotation, parent);


        /// <summary>
        /// Equitativo a usar <see cref="GameObject.GetComponent(Type)"/>
        /// </summary>
        protected Component GetComponent(Type type) => outer.GetComponent(type);

        /// <summary>
        /// Equitativo a usar <see cref="GameObject.GetComponent{T}()"/>
        /// </summary>
        protected T GetComponent<T>() => outer.GetComponent<T>();

        /// <summary>
        /// Equitativo a usar <see cref="GameObject.TryGetComponent{T}(out T)"/>
        /// </summary>
        protected bool TryGetComponent<T>(out T component)
        {
            return outer.TryGetComponent(out component);
        }

        /// <summary>
        /// Equitativo a usar <see cref="GameObject.TryGetComponent(Type, out Component)"/>
        /// </summary>
        protected bool TryGetComponent(Type componentType, out Component component)
        {
            return outer.TryGetComponent(componentType, out component);
        }

        /// <summary>
        /// Obtiene el <see cref="Animator"/> para este objeto, por defecto retorna null
        /// </summary>
        protected virtual Animator GetAnimator()
        {
            return null;
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>. la cual su duracion puede ser modificada usando <paramref name="playbackRateParam"/> y <paramref name="duration"/>. usando el animador conseguido por <see cref="GetAnimator"/>, 
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        /// <param name="playbackRateParam">Un parametro de animator que gobierna que tan rapido se reproduce la animacion</param>
        /// <param name="duration">La duracion que deberia durar la animacion</param>
        protected void PlayAnimation(string layerName, string animationStateName, string playbackRateParam, float duration)
        {
            if (duration <= 0f)
            {
                LogWarning("EntityState.PlayAnimation: Zero duration is not allowed.");
                return;
            }
            Animator modelAnimator = GetAnimator();
            if ((bool)modelAnimator)
            {
                PlayAnimationOnAnimator(modelAnimator, layerName, animationStateName, playbackRateParam, duration);
            }
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>. la cual su duracion puede ser modificada usando <paramref name="playbackRateParam"/> y <paramref name="duration"/>. usando el <paramref name="modelAnimator"/>, 
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        /// <param name="playbackRateParam">Un parametro de animator que gobierna que tan rapido se reproduce la animacion</param>
        /// <param name="duration">La duracion que deberia durar la animacion</param>
        protected static void PlayAnimationOnAnimator(Animator modelAnimator, string layerName, string animationStateName, string playbackRateParam, float duration)
        {
            modelAnimator.speed = 1f;
            modelAnimator.Update(0f);
            int layerIndex = modelAnimator.GetLayerIndex(layerName);
            if (layerIndex >= 0)
            {
                modelAnimator.SetFloat(playbackRateParam, 1f);
                modelAnimator.PlayInFixedTime(animationStateName, layerIndex, 0f);
                modelAnimator.Update(0f);
                float length = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
                modelAnimator.SetFloat(playbackRateParam, length / duration);
            }
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>. la cual su duracion puede ser modificada usando <paramref name="playbackRateParam"/> y <paramref name="duration"/>. usando el animador conseguido por <see cref="GetAnimator"/>.
        /// 
        /// <para>Esto funciona con una transicion entre la animacion actual y la nueva animacion, la transicion dura <paramref name="crossfadeDuration"/></para>
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        /// <param name="playbackRateParam">Un parametro de animator que gobierna que tan rapido se reproduce la animacion</param>
        /// <param name="duration">La duracion que deberia durar la animacion</param>
        /// <param name="crossfadeDuration">La duracion de la transicion</param>
        protected void PlayCrossfade(string layerName, string animationStateName, string playbackRateParam, float duration, float crossfadeDuration)
        {
            if (duration <= 0f)
            {
                LogWarning("EntityState.PlayCrossfade: Zero duration is not allowed.");
                return;
            }
            Animator modelAnimator = GetAnimator();
            if ((bool)modelAnimator)
            {
                modelAnimator.speed = 1f;
                modelAnimator.Update(0f);
                int layerIndex = modelAnimator.GetLayerIndex(layerName);
                modelAnimator.SetFloat(playbackRateParam, 1f);
                modelAnimator.CrossFadeInFixedTime(animationStateName, crossfadeDuration, layerIndex);
                modelAnimator.Update(0f);
                float length = modelAnimator.GetNextAnimatorStateInfo(layerIndex).length;
                modelAnimator.SetFloat(playbackRateParam, length / duration);
            }
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>. Creando una transicion entre la animacion actual y la nueva de una duracion de <paramref name="crossfadeDuration"/>. usando el animador conseguido por <see cref="GetAnimator"/>
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        /// <param name="crossfadeDuration">La duracion de la transicion</param>
        protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
        {
            Animator modelAnimator = GetAnimator();
            if ((bool)modelAnimator)
            {
                modelAnimator.speed = 1f;
                modelAnimator.Update(0f);
                int layerIndex = modelAnimator.GetLayerIndex(layerName);
                modelAnimator.CrossFadeInFixedTime(animationStateName, crossfadeDuration, layerIndex);
            }
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>, usando el animador conseguido por <see cref="GetAnimator"/>
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        public virtual void PlayAnimation(string layerName, string animationStateName)
        {
            Animator modelAnimator = GetAnimator();
            if ((bool)modelAnimator)
            {
                PlayAnimationOnAnimator(modelAnimator, layerName, animationStateName);
            }
        }

        /// <summary>
        /// Reproduce una animacion de nombre <paramref name="animationStateName"/> en el layer <paramref name="layerName"/>, usando el animador otorgado en <paramref name="modelAnimator"/>
        /// </summary>
        /// <param name="layerName">El nombre de la layer que tiene la animacion</param>
        /// <param name="animationStateName">el estado de la animacion</param>
        /// <param name="modelAnimator">El animador que esta animando</param>
        protected static void PlayAnimationOnAnimator(Animator modelAnimator, string layerName, string animationStateName)
        {
            int layerIndex = modelAnimator.GetLayerIndex(layerName);
            modelAnimator.speed = 1f;
            modelAnimator.Update(0f);
            modelAnimator.PlayInFixedTime(animationStateName, layerIndex, 0f);
        }

        /// <summary>
        /// Escribe un mensaje de log en la consola
        /// </summary>
        /// <param name="message">El mensaje</param>
        /// <param name="memberName">El metodo que esta llamado este metodo, no es necesario dar esta informacion porque es generada por el compilador</param>
        protected void Log(object message, [CallerMemberName] string memberName = "")
        {
            Debug.Log($"[{stateName}.{memberName}]: {message} (Type=\"{fullStateName}\")");
        }

        /// <summary>
        /// Escribe un mensaje de log de tipo Warning en la consola
        /// </summary>
        /// <param name="message">El mensaje</param>
        /// <param name="memberName">El metodo que esta llamado este metodo, no es necesario dar esta informacion porque es generada por el compilador</param>
        protected void LogWarning(object message, [CallerMemberName] string memberName = "")
        {
            Debug.LogWarning($"[{stateName}.{memberName}]: {message} (Type=\"{fullStateName}\")");
        }

        /// <summary>
        /// Escribe un mensaje de log de tipo Error en la consola
        /// </summary>
        /// <param name="message">El mensaje</param>
        /// <param name="memberName">El metodo que esta llamado este metodo, no es necesario dar esta informacion porque es generada por el compilador</param>
        protected void LogError(object message, [CallerMemberName] string memberName = "")
        {
            Debug.LogError($"[{stateName}.{memberName}]: {message} (Type=\"{fullStateName}\")");
        }

        /// <summary>
        /// Metodo que se deberia usar para inicializar el estado.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Constructor generico de estado
        /// </summary>
        public State()
        {
            Initialize();
            Type type = GetType();
            stateName = type.Name;
            fullStateName = type.FullName;
        }
    }
}