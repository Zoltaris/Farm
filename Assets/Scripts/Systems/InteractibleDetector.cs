using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDetector : MonoBehaviour
{
    public Interactable _currentInteractable;
    public GameObject _currentInteractiveObject;
    public CharacterControl _cc { set; private get; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Interactable>())
        {
            _currentInteractiveObject = other.gameObject;
            _currentInteractable = other.GetComponentInParent<Interactable>();
            if (_currentInteractable is IInteractable)
            {
                _cc._currentInteractable = _currentInteractable as IInteractable;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _currentInteractiveObject)
        {
            _currentInteractiveObject = null;
            _currentInteractable = null;
            _cc._currentInteractable = null;
        }
            
    }
}
