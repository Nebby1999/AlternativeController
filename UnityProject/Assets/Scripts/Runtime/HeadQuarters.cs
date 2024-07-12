using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

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
            //TrySupplyBases(MineralType.Black);
            //TrySupplyBases(MineralType.Red);
        }
        private void ChangePlayersState()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                vehicles[i].IsBattle = _input.GetPlayerSwitch(i);
            }
        }

        public void TryLoadResource(ResourceDef resource, int amount) => TryLoadResource(resource.resourceIndex, amount);

        public void TryLoadResource(ResourceIndex index, int amount)
        {

        }

        /*private void TrySupplyBases(MineralType mineral)
        {
            for (int i = 0; i < _bases.Length; i++)
            {
                bool input = false;
                if(mineral == MineralType.Black)
                {
                    //Base is "Black" base
                    if(i == 0)
                    {
                        input = _input.GetCableInput(0);
                    }
                    //Current base is red base
                    else
                    {
                        input = _input.GetCableInput(3);
                    }
                }
                else //mineral is "Red"
                {
                    //Base is "Red" base
                    if (i == 1)
                    {
                        input = _input.GetCableInput(5);
                    }
                    //Base is "Black" base
                    else
                    {
                        input = _input.GetCableInput(2);
                    }
                }

                if(input && _resources.UnloadResource(mineral, 0.04f)) 
                    _bases[i].TryLoadMineral(mineral, 0.04f);
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
        }*/
    }
}
