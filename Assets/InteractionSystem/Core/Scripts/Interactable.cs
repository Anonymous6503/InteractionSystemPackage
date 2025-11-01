using System;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionType
{
    OneShot,Toggle
}

public class Interactable : MonoBehaviour, IInteractable
{
    [Tooltip("The type of interaction this particular interactable has")]
    [SerializeField] private InteractionType _interactionType =  InteractionType.Toggle;
    
    [Tooltip("Fires ONCE for the OneShot type.")]
    [SerializeField] private UnityEvent OnInteractOneShot;

    [Tooltip("Fires when Toggled ON.")]
    [SerializeField] private UnityEvent OnToggleOn;

    [Tooltip("Fires when Toggled OFF.")]
    [SerializeField] private UnityEvent OnToggleOff;
    
    
    [SerializeField] private bool _isToggled = false;
    [SerializeField] private bool _isUsedOnce = false;

    public void Interact()
    {
        switch (_interactionType)
        {
            case InteractionType.OneShot:
                if (!_isUsedOnce)
                {
                    Debug.Log("Interacting: OneShot");
                    OnInteractOneShot.Invoke();
                    _isUsedOnce = true; 
                }
                else
                {
                    Debug.Log("Interacting: OneShot (Already Used)");
                }
                break;

            case InteractionType.Toggle:
                if (!_isToggled)
                {
                    Debug.Log("Interacting: Toggle ON");
                    OnToggleOn.Invoke();
                    _isToggled = true;
                }
                else
                {
                    Debug.Log("Interacting: Toggle OFF");
                    OnToggleOff.Invoke();
                    _isToggled = false;
                }
                break;
        }
    }
}
