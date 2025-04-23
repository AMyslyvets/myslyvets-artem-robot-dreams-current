using System.Runtime.CompilerServices;
using UnityEngine;

namespace Lesson_15.MainMenu.Source
{
    public class HashTest : MonoBehaviour
    {
        public class TestToo
        {
            
        }
        
        [SerializeField] private Collider _collider;

        private TestToo testToo = new();

        [ContextMenu("Test")]
        private void Test()
        {
            int unityHash = _collider.GetHashCode();
            int managedHash = RuntimeHelpers.GetHashCode(_collider);
            Debug.Log($"Unity hash: {unityHash}\nManaged hash: {managedHash}");
        }
        
        [ContextMenu("Test 2")]
        private void Test2()
        {
            int unityHash = testToo.GetHashCode();
            int managedHash = RuntimeHelpers.GetHashCode(testToo);
            Debug.Log($"Unity hash: {unityHash}\nManaged hash: {managedHash}");
        }
    }
}