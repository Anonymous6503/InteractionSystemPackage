using System.Collections;
using System.Threading.Tasks;
using InteractionSystem.Runtime.Strategy;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class InteractionStrategy_DelayInteract : InteractionStrategy
    {
        [SerializeField] private string _strategyName = "Delay Interaction";

        [Tooltip("The amount of delay between interactions.")] [SerializeField]
        private float _delayAmount = 3f;


        [SerializeField] private UnityEvent OnInteract;

        public override async void Execute(GameObject gameObjectToInteractWith)
        {
            if (hasConditions)
            {
                foreach (var ic in allConditions)
                {
                    if (!ic.isConditionsMet())
                        return;
                }
            }
            Debug.Log("Starting Delay Interaction");
            await Task.Delay(Mathf.RoundToInt(_delayAmount * 1000));
            OnInteract.Invoke();
            Debug.Log("Finished Delay Interaction");
        }

    }
}