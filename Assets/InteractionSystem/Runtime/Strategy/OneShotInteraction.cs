using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OneShotInteraction : InteractionStrategy
{
    [Tooltip("Only used if the interaction action is used.")]
    [SerializeField] private string _strategyName = "OneShot Interaction Strategy";
    bool _isUsed = false;

    public UnityEvent OnInteractionAction; 
    public override void Execute(GameObject gameObjectToInteractWith)
    {
        if (!_isUsed)
        {
            _isUsed = !_isUsed;
            Debug.Log("One Shot Interaction with: " + gameObjectToInteractWith.name);
            OnInteractionAction.Invoke();
            
            var interactable = gameObjectToInteractWith.GetComponent<Interactable>();
            if (interactable != null)
                GameObject.Destroy(interactable);
        }
    }
}
