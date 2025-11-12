
using System.Collections.Generic;
using InteractionSystem.Runtime.InteractionConditions;
using InteractionSystem.Runtime.Strategy;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem.Runtime
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [Tooltip("The current interaction Type on this particular interactable.")] [SerializeReference]
        public InteractionStrategy interactionType;
        
        [SerializeField] private UnityEvent OnFocusGained, OnFocusLost;

        public void Interact()
        {
            interactionType.Execute(this.gameObject);
        }

        public void GainFocus()
        {
            OnFocusGained.Invoke();
            GetInteractionPrompt();
        }

        public void LoseFocus()
        {
            OnFocusLost.Invoke();
        }

        public string GetInteractionPrompt()
        {
            return interactionType.interactionPrompt;
        }
    }
}