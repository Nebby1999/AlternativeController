using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa un banco de inputs de un <see cref="GameObject"/>, el cual suele ser un <see cref="CharacterBody"/>. Estos input son usados despues por distintos componentes del personaje.
    /// 
    /// <br></br>
    /// Para mas informacion sobre el sistema de Maestros y Cuerpos, mira <see cref="CharacterBody"/> y <see cref="CharacterMaster"/>
    /// </summary>
    public class InputBank : MonoBehaviour
    {
        [Tooltip("El input de movimiento de este objeto")]
        public Vector2 movementInput;
        [Tooltip("El input de rotacion de este objeto")]
        public int rotationInput;

        /// <summary>
        /// El boton primario para este objeto, usualmente ejecuta la habilidad ligada a <see cref="SkillSlot.Primary"/>
        /// </summary>
        public Button primaryButton;

        /// <summary>
        /// El boton segundario para este objeto, usualmente ejecuta la habilidad ligada a <see cref="SkillSlot.Secondary"/>
        /// </summary>
        public Button secondaryButton;

        /// <summary>
        /// El boton Especial para este objeto, usualmente ejecuta la habilidad ligada a <see cref="SkillSlot.Special"/>
        /// </summary>
        public Button specialButton;

        /// <summary>
        /// Representa un boton de Input
        /// </summary>
        public struct Button
        {
            /// <summary>
            /// El boton esta actualmente presionado?
            /// </summary>
            public bool down;

            /// <summary>
            /// El boton estaba presionado recien?
            /// </summary>
            public bool wasDown;
            public bool hasPressBeenClaimed;

            /// <summary>
            /// El boton recien fue soltado?
            /// </summary>
            public bool justReleased
            {
                get
                {
                    if (!down)
                        return wasDown;
                    return false;
                }
            }

            /// <summary>
            /// El boton fue recien presionado?
            /// </summary>
            public bool justPressed
            {
                get
                {
                    if (down)
                        return !down;
                    return false;
                }
            }

            /// <summary>
            /// Presiona o suelta el boton
            /// </summary>
            /// <param name="newState">True si el boton esta presionado, de lo contrario, false.</param>
            public void PushState(bool newState)
            {
                hasPressBeenClaimed &= newState;
                wasDown = down;
                down = newState;
            }
        }
    }
}