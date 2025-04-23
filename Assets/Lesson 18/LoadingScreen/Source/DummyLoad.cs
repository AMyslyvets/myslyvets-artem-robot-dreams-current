using UnityEngine;

namespace LoadingScreen
{
    [CreateAssetMenu(fileName = "DummyLoad", menuName = "Data/Loading/DummyLoad")]
    [PreferBinarySerialization]
    public class DummyLoad : ScriptableObject
    {
        [SerializeField, Tooltip("Size in MB")] private int _size;
        
        [SerializeField, HideInInspector] private byte[] _data;

        public int Size => _data.Length;
        
        [ContextMenu("Generate Data")]
        private void GenerateData()
        {
            int byteCount = _size * 1024 * 1024;
            _data = new byte[byteCount];
            for (int i = 0; i < _data.Length; ++i)
            {
                _data[i] = (byte)Random.Range(0, 255);
            }
        }
    }
}