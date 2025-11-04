using UnityEngine;
using UnityEngine.Events;

public class InteractionStrategy_FixedCount : InteractionStrategy
{
    [SerializeField] private string _strategyName = "Fixed Count Strategy";

    [SerializeField] private UnityEvent OnInteraction; 
    
    [Tooltip("Total number of times interaction will be availabale for")]
    [SerializeField] private int _totalCount = 3;

    
    public override void Execute(GameObject gameObjectToInteractWith)
    {
        if (_totalCount > 0)
        {
            OnInteraction.Invoke();
            _totalCount--;
            Debug.Log("Used Once");
            Debug.Log("Remaining Count: " + (_totalCount));
        }
    }
}
