using System;
using UnityEngine;

namespace AC
{

    [Obsolete]
    public interface IInputable
    {
        Vector2 GetDirection();
        bool IsFireButtonPressed();
    }
}