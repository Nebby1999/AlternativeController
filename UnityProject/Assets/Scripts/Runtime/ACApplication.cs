using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Represents the main application of the game, with important instance based data. Works as a Singleton
    /// </summary>
    public class ACApplication : MonoBehaviour
    {
        /// <summary>
        /// Returns the current instance of the Application
        /// </summary>
        public static ACApplication instance { get; private set; }

        /// <summary>
        /// Event that's fired on Update
        /// </summary>
        public event Action onUpdate;

        /// <summary>
        /// Event that's fired on FixedUpdate
        /// </summary>
        public event Action onFixedUpdate;

        /// <summary>
        /// Event that's fired on LateUpdate
        /// </summary>
        public event Action onLateUpdate;

        /// <summary>
        /// Event that's fired when the game has loaded
        /// </summary>
        public event Action onLoad;

        /// <summary>
        /// Event that's fired when the game quits
        /// </summary>
        public event Action onQuit;

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            InitializeGame();
        }

        private void Update()
        {
            onUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            onFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            onQuit?.Invoke();
        }

        private void InitializeGame()
        {
            ResourceCatalog.Initialize();
            onLoad?.Invoke();
        }
    }
}