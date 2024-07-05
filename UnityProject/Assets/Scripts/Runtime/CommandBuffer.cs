using System.Collections.Generic;
using UnityEngine;

namespace AC
{ 
    [CreateAssetMenu(menuName = "Command Buffer")]
    public class CommandBuffer : ScriptableObject
    {
        private Queue<Command> _queue = new Queue<Command>();
        private Command _activeCommand;
        public void ExecuteQueue()
        {
            if(_queue.Count <= 0) return;

            _activeCommand = _queue.Dequeue();

            if(_activeCommand != null && !_activeCommand.IsExecuting)
            {
                _activeCommand.OnExecutionComplete += OnActiveCommandComplete;
                _activeCommand.Execute();
            }
        }
        private void OnActiveCommandComplete()
        {
            _activeCommand.OnExecutionComplete -= OnActiveCommandComplete;
            _activeCommand = null;
        }
        public void QueueCommand(Command command)
        {
            Debug.Log($"{_queue.Count}");

            // if (_activeCommand?.GetType() == command.GetType())
            // {
            //     Debug.Log("El nuevo comando es del mismo tipo que el comando en la parte superior de la cola. No se encolará.");
            // }
            // else
            // {
                _queue.Enqueue(command);
            // }
        }
    }
}