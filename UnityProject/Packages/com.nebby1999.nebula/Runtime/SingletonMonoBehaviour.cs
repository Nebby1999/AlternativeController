using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Representa un <see cref="MonoBehaviour"/> que solo una instancia de este debe existir en cualquier momento.
    /// </summary>
    /// <typeparam name="T">El tipo de monobehaviour</typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// La instancia del singleton
        /// </summary>
        public static T instance { get; private set; }

        /// <summary>
        /// Si este valor es verdadero, cualquier isntancia nueva creada mientras <see cref="instance"/> tenga un valor valido sera destruida
        /// </summary>
        protected virtual bool destroyIfDuplicate { get; } = false;

        /// <summary>
        /// Asigna el valor de <see cref="instance"/>, o destruye el componente si <see cref="destroyIfDuplicate"/> es verdarero
        /// </summary>
        protected virtual void OnEnable()
        {
            if(instance != null && instance != this)
            {
                Debug.LogWarning($"Duplicate instance of {typeof(T).Name} detected. Only a single instance should exist at a time! " + (destroyIfDuplicate ? "Destroying Duplicate." : "Replacing instance with new one."), this);

                if(destroyIfDuplicate)
                {
                    DestroySelf();
                    return;
                }
            }
            instance = this as T;
        }

        /// <summary>
        /// Destruye el componente cuando <see cref="destroyIfDuplicate"/> es verdadero
        /// </summary>
        protected virtual void DestroySelf()
        {
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
#else
            Destroy(gameObject);
#endif
        }

        /// <summary>
        /// Libera la <see cref="instance"/> actual
        /// </summary>
        protected virtual void OnDisable()
        {
            if(instance == this)
            {
                instance = null;
            }
        }
    }
}