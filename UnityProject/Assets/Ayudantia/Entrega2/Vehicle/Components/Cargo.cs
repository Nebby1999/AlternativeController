using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Cargo
{
    private readonly Queue<MineralType> _shipment;
    private readonly int _maxCount;
    public Queue<MineralType> Shipment => _shipment;
    [SerializeField] private int _red;
    [SerializeField] private int _black;
    public int Count => _shipment.Count;
    public MineralType LastUnloadedMineral {get; private set;}
    public Cargo(int capacity)
    {
        _shipment = new Queue<MineralType>(capacity);
        _maxCount = capacity;
    }
    public bool Load(MineralType type, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(_shipment.Count >= _maxCount) return false;
            _shipment.Enqueue(type);
            Debug.Log($"{type} Loaded");
            if (type == MineralType.Red) _red++;
            else if (type == MineralType.Black) _black++;
        }
        Debug.Log($"{type} Loaded (x{amount})");
        Debug.Log($"Cargo Count: {Count}");
        return true;
    }
    public bool Unload(int amount)
    {
        if (_shipment.Count < 1) return false;
        for (int i = 0; i < amount; i++)
        {
            LastUnloadedMineral = _shipment.Peek();
            if (LastUnloadedMineral == MineralType.Red) _red--;
            else if (LastUnloadedMineral == MineralType.Black) _black--;
            Debug.Log($"{LastUnloadedMineral} Unloaded");
            _shipment.Dequeue();
        }
        return true;
    }
}
