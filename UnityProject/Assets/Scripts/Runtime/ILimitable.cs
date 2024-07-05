using UnityEngine;

namespace AC
{

    public interface ILimitable
    {
        Vector2 ApplyLimit(Vector2 newPosition, float offset);
    }
}