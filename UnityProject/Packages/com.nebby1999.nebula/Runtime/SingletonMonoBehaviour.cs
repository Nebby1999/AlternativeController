using UnityEngine;

namespace Nebula
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T instance { get; private set; }

        protected virtual void OnEnable()
        {
            if(instance != null)
            {
                Debug.LogWarning($"Duplicate instance of {this} detected. replacing static instance with new instance.", this);
            }
            instance = this as T;
        }

        protected virtual void OnDisable()
        {
            if(instance == this)
            {
                instance = null;
            }
        }
    }
}