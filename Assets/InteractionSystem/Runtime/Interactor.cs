using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        [Tooltip("The radius in which the interactor will find all the possible interactable.")] 
        [SerializeField] private float interactionRadius;

        [Tooltip("The layer which  we interacting with.")] [SerializeField]
        private LayerMask interactableLayer;

        private Collider[] colliderBuffer = new Collider[20];
        private int numFound;

        [Tooltip("The currently selected interactable with which the player will interact with.")] 
        [SerializeField] private IInteractable focusedInteractable;

        private void FixedUpdate()
        {
            GetFocusedInteractable();
        }

        /// <summary>
        /// Check and returns the _focusedInteractable within the _interaction radius
        /// </summary>
        private void GetFocusedInteractable()
        {
            numFound = Physics.OverlapSphereNonAlloc(this.transform.position, interactionRadius, colliderBuffer,
                interactableLayer);

            if (numFound == 0)
            {
                Debug.Log("No interactable found");
                if(focusedInteractable != null)
                    focusedInteractable.LoseFocus();
                focusedInteractable = null;
                return;
            }

            IInteractable closestInteractable = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < numFound; i++)
            {
                Collider col = colliderBuffer[i];

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
                focusedInteractable = null;
                return;
            }

            Debug.Log("Found interactable: " + closestInteractable);

            if (focusedInteractable != closestInteractable)
            {
                if(focusedInteractable != null)
                    focusedInteractable.LoseFocus();
                focusedInteractable = closestInteractable;
                focusedInteractable.GainFocus();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (focusedInteractable != null)
                {
                    focusedInteractable.Interact();
                }
            }
        }
    }
}