using UnityEngine;

namespace AC
{

    public interface IInputable
    {
        Vector2 GetDirection();
        bool IsFireButtonPressed();
    }
}