using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace AC
{

    public class Collect : Command
    {
        private readonly Cargo_OLD _cargo;
        private readonly MacroKeyboard _input;
        private readonly IHarvesteable_OLD _item;
        private readonly int _delay;
        public Collect(Cargo_OLD cargo, MacroKeyboard input, IHarvesteable_OLD item, int delayInSeconds)
        {
            _cargo = cargo;
            _input = input;
            _item = item;
            _delay = delayInSeconds *= 1000;
        }
        protected override async Task AsyncExecuter()
        {
            int amount = _item is Mineral ? 1 : 2;
            _cts = new CancellationTokenSource();
            Debug.Log("Esperando");
            try
            {
                await Task.Delay(_delay, _cts.Token);
                if(_cts.Token.IsCancellationRequested) return;
                if(!_input.GetAction(1))
                {
                    _cts?.Cancel();
                    return;
                }
                if(_cargo.LoadResource(_item.resourceType, amount)) _item.Harvest(amount);
            }
            catch(TaskCanceledException)
            {
                Debug.Log("Cancelando Collect");
            }
        }
    }
}
