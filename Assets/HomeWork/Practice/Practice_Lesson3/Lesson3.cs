using UnityEngine;

public class Lesson3 : MonoBehaviour
{
    [SerializeField] private int _firstInterNumber;
    [SerializeField] private int _secondInterNumber;
    [SerializeField] private float _firstFloatNumber;
    [SerializeField] private string _firstText;
    [SerializeField] private bool _fistCheck;
    [SerializeField] private My2Vector _fistVector;
    
    private float _hidenNumber;
    
    
    [ContextMenu("Hello World")]
    private void HelloWorld()
    {
        Debug.Log("Hello World");
    }

    [ContextMenu("Add")]
    private void Add()
    {
        int result = _firstInterNumber + _secondInterNumber;
        Debug.Log(result);
    }
}