using System;
using UnityEngine;

namespace AC
{

    [Obsolete]
    public interface ILimitable
    {
        Vector2 ApplyLimit(Vector2 newPosition, float offset);
    }
}