using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour, IInteractable
{
    [Tooltip("The current interaction Type on this particular interactable.")]
    [SerializeReference] public InteractionStrategy _interactionType;

    private void Start()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact()
    {
        _interactionType.Execute(this.gameObject);
    }
}
