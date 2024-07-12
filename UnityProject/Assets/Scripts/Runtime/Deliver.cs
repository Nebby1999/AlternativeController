using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace AC
{

    public class Deliver : Command
    {
        private readonly Cargo _cargo;
        private readonly MacroKeyboard _input;
        private readonly HeadQuarters_OLD _hq;
        private readonly int _delay;
        public Deliver(Cargo cargo, MacroKeyboard input, HeadQuarters_OLD hq, int delayInSeconds)
        {
            _cargo = cargo;
            _input = input;
            _hq = hq;
            _delay = delayInSeconds *= 1000;
        }
        protected override async Task AsyncExecuter()
        {
            _cts = new CancellationTokenSource();
            Debug.Log("Esperando");
            try
            {
                await Task.Delay(_delay, _cts.Token);
                if(_cts.Token.IsCancellationRequested) return;
                if(!_input.GetAction(0))
                {
                    _cts?.Cancel();
                    return;
                }
                if(!_cargo.UnloadResource(1)) return;
                _hq.TryLoadResource(_cargo.lastUnloadedResource, 1);
                Debug.Log($"Delivering {_cargo.lastUnloadedResource} into HQ");
            }
            catch(TaskCanceledException)
            {
                Debug.Log("Cancelando Deliver");
            }
        }
    }
}
