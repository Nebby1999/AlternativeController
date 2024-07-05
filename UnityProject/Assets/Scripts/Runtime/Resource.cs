using System;
using UnityEngine;

namespace AC
{

    [Serializable]
    public class Resource
    {
        public readonly MineralType Type;
        [SerializeField] private float value;
        public float Value => value;
        public Resource(MineralType mineral)
        {
            Type = mineral;
        }
        public void Add(float amount)
        {
            value += amount;
        }
        public void Substract(float amount)
        {
            value = Mathf.Max(0, value - amount);
        }
    }
}