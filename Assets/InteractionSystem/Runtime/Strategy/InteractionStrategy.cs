using System.Collections.Generic;
using InteractionSystem.Runtime.InteractionConditions;
using UnityEngine;

namespace InteractionSystem.Runtime.Strategy
{
    [System.Serializable]
    public abstract class  InteractionStrategy
    {
        public string interactionPrompt = "Interact";
        
        [Header("Conditions")]
        [Tooltip("Does this Interaction have any condition to check?")]
        [SerializeField] protected bool hasConditions;
        
        [SerializeReference]
        public List<InteractionCondition> allConditions = new  List<InteractionCondition>();

        protected InteractionStrategy()
        {
            if(allConditions == null)
                allConditions = new List<InteractionCondition>();
        }
        
        public abstract void Execute(GameObject gameObjectToInteractWith);
    }
}