using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace InteractionSystem.Runtime.Strategy
{
    [System.Serializable]
    public class InteractionStrategy_Once : InteractionStrategy
    {
        [Tooltip("Only used if the interaction action is used.")] [SerializeField]
        private string strategyName = "OneShot Interaction Strategy";

        [Tooltip("Check if this interaction strategy is used or not")]
        bool isUsed = false;

        [Tooltip("Removes the Interactable component from the assigned Game object.")]
        [SerializeField] private bool deleteAfterInteraction = false;

        public UnityEvent OnInteractionAction;

        public override void Execute(GameObject gameObjectToInteractWith)
        {
            if (hasConditions)
            {
                foreach (var ic in allConditions)
                {
                    if (!ic.isConditionsMet())
                        return;
                }
            }
            
            if (!isUsed)
            {
                isUsed = !isUsed;
                Debug.Log("One Shot Interaction with: " + gameObjectToInteractWith.name);
                OnInteractionAction.Invoke();


                // Delete the Interaction strategy once it is used, and if the user wants to.
                if (deleteAfterInteraction)
                {
                    var interactable = gameObjectToInteractWith.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        Object.Destroy(interactable);
                    }
                }
            }
        }
    }
}