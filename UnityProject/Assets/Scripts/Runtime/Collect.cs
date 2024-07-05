using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace AC
{

    public class Collect : Command
    {
        private readonly Cargo _cargo;
        private readonly MacroKeyboard _input;
        private readonly IHarvesteable _item;
        private readonly int _delay;
        public Collect(Cargo cargo, MacroKeyboard input, IHarvesteable item, int delayInSeconds)
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
                if(_cargo.Load(_item.Type, amount)) _item.Harvest(amount);
            }
            catch(TaskCanceledException)
            {
                Debug.Log("Cancelando Collect");
            }
        }
    }
}
