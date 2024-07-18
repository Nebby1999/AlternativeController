using UnityEngine;

namespace AC
{
    public class InputBank : MonoBehaviour
    {
        public Vector2 movementInput;
        public int rotationInput;
        public Button primaryButton;
        public Button secondaryButton;

        public struct Button
        {
            public bool down;
            public bool wasDown;
            public bool hasPressBeenClaimed;
            public bool justReleased
            {
                get
                {
                    if (!down)
                        return wasDown;
                    return false;
                }
            }

            public bool justPressed
            {
                get
                {
                    if (down)
                        return !down;
                    return false;
                }
            }

            public void PushState(bool newState)
            {
                hasPressBeenClaimed &= newState;
                wasDown = down;
                down = newState;
            }
        }
    }
}