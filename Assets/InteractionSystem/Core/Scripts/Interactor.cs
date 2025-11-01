using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    [SerializeField] private float _interactionRadius;
    [SerializeField] private LayerMask _interactableLayer;
    
    private readonly Collider[] _colliderBuffer = new Collider[20];
    private int _numFound;
    
    [SerializeField] private IInteractable _focusedInteractable;

    private void FixedUpdate()
    {
        GetFocusedInteractable();
    }

    private void GetFocusedInteractable()
    {
        _numFound = Physics.OverlapSphereNonAlloc(this.transform.position, _interactionRadius, _colliderBuffer, _interactableLayer);

        if (_numFound == 0)
        {
            Debug.Log("No interactables found");
            _focusedInteractable = null;
            return;
        }

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < _numFound; i++)
        {
            Collider col = _colliderBuffer[i];
            
            if (col.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }
        
        if (closestInteractable == null)
        {
            _focusedInteractable = null;
            return;
        }
        Debug.Log("Found interactable: " + closestInteractable);
        
        _focusedInteractable = closestInteractable;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_focusedInteractable != null)
            {
                _focusedInteractable.Interact();
            }
        }
    }
}
