using System;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado usado para probar las capacidades de configuracion de un <see cref="Nebula.StateConfiguration"/>
    /// </summary>
    public class StateConfigurationTest : State
    {
        /// <summary>
        /// LayerMask a serializar
        /// </summary>
        [Tooltip("LayerMask a serializar")]
        public static LayerMask testLayerMask;
        [Tooltip("Character a serilaizar")]
        public static char testChar;
        [Tooltip("Quaternion a serializar")]
        public static Quaternion testQuaternion;

        protected override void Initialize()
        {
        }
    }
}