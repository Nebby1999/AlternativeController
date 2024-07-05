using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Supply : Command
{
    private readonly ResourcesManager _sender;
    private readonly ResourcesManager _receiver;
    private readonly MineralType _mineral;
    private readonly BoxInputs _input;
    private readonly float _amount;
    private readonly int _cable;
    private readonly int _delay;
    public Supply(ResourcesManager sender, ResourcesManager receiver, MineralType mineral, BoxInputs input, float amount, int cable, int delay)
    {
        _sender = sender;
        _receiver = receiver;
        _mineral = mineral;
        _input = input;
        _amount = amount;
        _cable = cable;
        _delay = delay;
    }
    protected override async Task AsyncExecuter()
    {
        _cts = new CancellationTokenSource();
        Debug.Log("Esperando");
        try
        {
            await Task.Delay(_delay, _cts.Token);
            if(_cts.Token.IsCancellationRequested) return;
            if(!_input.GetCableInput(_cable))
            {
                Debug.LogWarning("HQ cables are not set");
                _cts?.Cancel();
                return;
            }
            if(!_sender.UnloadMineral(_mineral, _amount))
            {
                Debug.LogWarning($"Desired amount exceeds current value of resources from sender");
                _cts?.Cancel();
                return;
            }
            _receiver.LoadMaterial(_mineral, _amount);
            Debug.Log($"Delivering {_amount} of {_mineral} into ");
        }
        catch(TaskCanceledException)
        {
            Debug.Log("Cancelando Deliver");
        }
    }
}
