using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC
{
    public class CoroutineTask : IEnumerator
    {
        private IEnumerator _internalCoroutine;

        public object Current => _internalCoroutine?.Current;

        public bool MoveNext()
        {
            return _internalCoroutine?.MoveNext() ?? false;
        }

        public void Reset()
        {
            _internalCoroutine?.Reset();
        }


        public CoroutineTask(IEnumerator internalCoroutine)
        {

        }
    }
}