using InteractionSystem;
using InteractionSystem.Runtime.InteractionConditions;
using UnityEngine;

public class IC_GameobjectCheck : InteractionCondition
{
    [SerializeField] public GameObjectCondition condition;
    [SerializeField] public GameObject requiredGameObject;
    
    public override bool isConditionsMet()
    {
        if (condition.gameObject == requiredGameObject)
            return true;
        return false;
    }
}
