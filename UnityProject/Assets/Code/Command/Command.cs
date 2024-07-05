using System;
using System.Threading;
using System.Threading.Tasks;
public abstract class Command
{
    private bool _isExecuting = false;
    public bool IsExecuting => _isExecuting;
    public event Action OnExecutionComplete;
    protected CancellationTokenSource _cts;
    public async void Execute()
    {
        _isExecuting = true;
        await AsyncExecuter();
        _isExecuting = false;
        OnExecutionComplete?.Invoke();
    }
    protected abstract Task AsyncExecuter();
}
