using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado generico de un Stun.
    /// </summary>
    public class GenericStunState : EntityState
    {
        /// <summary>
        /// La duracion del stun
        /// </summary>
        protected float stunDuration;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(fixedAge > stunDuration) //Stun se acabo, volver a main.
            {
                outer.SetNextStateToMain();
            }
        }

        /// <summary>
        /// Constructor sin parametros de un stun, la duracion de stun es 1 segundo.
        /// </summary>
        public GenericStunState() 
        {
            stunDuration = 1f;
        }

        /// <summary>
        /// Constructor de un stun, la duracion del stun es colocado por <paramref name="stunDuration"/>
        /// </summary>
        /// <param name="stunDuration">La duracion deseada del stun</param>
        public GenericStunState(float stunDuration)
        {
            this.stunDuration = stunDuration;
        }
    }
}