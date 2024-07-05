using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxInputs))]
[RequireComponent(typeof(ResourcesManager))]
public class HeadQuarters : MonoBehaviour
{
    private BoxInputs _input;
    private ResourcesManager _resources;
    [SerializeField] private Vehicle[] vehicles;
    [SerializeField] private CommandBuffer _hqBuffer;
    [SerializeField] private Base[] _bases;
    private void Awake()
    {
        _input = GetComponent<BoxInputs>();
        _resources = GetComponent<ResourcesManager>();
    }
    private void Update()
    {
        ChangePlayersState();
    }
    private void FixedUpdate()
    {
        TrySupplyBases(MineralType.Black);
        TrySupplyBases(MineralType.Red);
    }
    private void ChangePlayersState()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].IsBattle = _input.GetPlayerSwitch(i);
        }
    }
    private void TrySupplyBases(MineralType mineral)
    {
        for (int i = 0; i < _bases.Length; i++)
        {
            bool input = mineral == MineralType.Black ? (i == 0 ? _input.GetCableInput(0) : _input.GetCableInput(3)) : i == 1 ? _input.GetCableInput(5) : _input.GetCableInput(2);
            Debug.Log(input);
            if(input && _resources.UnloadMineral(mineral, 0.04f)) _bases[i].TryLoadMineral(mineral, 0.04f);
        }
    }
    public void TryLoadMineral(MineralType mineral, int amount)
    {
        bool input = mineral == MineralType.Black ? _input.GetCableInput(1) : _input.GetCableInput(4);
        if(!input)
        {
            Debug.LogWarning("HQ cables are not set");
            return;
        }
        _resources.LoadMaterial(mineral, amount);
    }
}
