using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    [Tooltip("The radius in which the interactor will find all the possible interactables.")]
    [SerializeField] private float _interactionRadius;
    
    [Tooltip("The layer which  we interacting with.")]
    [SerializeField] private LayerMask _interactableLayer;
    
    private readonly Collider[] _colliderBuffer = new Collider[20];
    private int _numFound;
    
    [Tooltip("The currently selected interactable with which the player will interact with.")]
    [SerializeField] private IInteractable _focusedInteractable;

    private void FixedUpdate()
    {
        GetFocusedInteractable();
    }

    /// <summary>
    /// Check and returns the _focusedInteractable within the _interaction radius
    /// </summary>
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
