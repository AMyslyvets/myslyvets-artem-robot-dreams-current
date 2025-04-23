using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject _wholeBox;
    [SerializeField] private GameObject[] _breakStages;
    [SerializeField] private GameObject _brokenStage;
    
    [SerializeField] private Rigidbody[] _parts;
    [SerializeField] private float _breakForce;
    
    [SerializeField] private InputAction _hitAction;

    private int _hits;

    private void Awake()
    {
        _brokenStage.SetActive(false);

        for (int i = 1; i < _breakStages.Length; ++i)
        {
            _breakStages[i].SetActive(false);
        }
        
        _breakStages[0].SetActive(true);
        _wholeBox.SetActive(true);
        
        _hitAction.Enable();
        _hitAction.performed += HitHandler;
    }

    private void HitHandler(InputAction.CallbackContext context)
    {
        Hit();
    }
    
    [ContextMenu("Hit")]
    public void Hit()
    {
        _breakStages[_hits].SetActive(false);
        _hits++;
        if (_hits < _breakStages.Length)
        {
            _breakStages[_hits].SetActive(true);
        }
        else
        {
            _wholeBox.SetActive(false);
            _brokenStage.SetActive(true);
            Vector3 center = transform.position;
            for (int i = 0; i < _parts.Length; ++i)
            {
                Vector3 direction = (_parts[i].transform.position - center).normalized;
                _parts[i].AddForce(direction * _breakForce, ForceMode.Impulse);
            }
        }
    }
}
