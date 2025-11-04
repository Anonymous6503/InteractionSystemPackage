using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class InteractionStrategy_DelayInteract : InteractionStrategy
{
    [SerializeField] private string  _strategyName = "Delay Interaction";
    
    [Tooltip("The amount of delay between interactions.")]
    [SerializeField] private float _delayAmount = 3f;
    
    
    [SerializeField] private UnityEvent OnInteract;
    public override async void Execute(GameObject gameObjectToInteractWith)
    {
        Debug.Log("Starting Delay Interaction");
        await Task.Delay(Mathf.RoundToInt(_delayAmount * 1000));
        OnInteract.Invoke();
        Debug.Log("Finished Delay Interaction");
    }

}
