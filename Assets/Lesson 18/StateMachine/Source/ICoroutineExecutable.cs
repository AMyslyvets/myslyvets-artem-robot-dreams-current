using System.Collections;

namespace StateMachineSystem
{
    public interface ICoroutineExecutable
    {
        void ExecuteCoroutine(IEnumerator routine);
    }
}