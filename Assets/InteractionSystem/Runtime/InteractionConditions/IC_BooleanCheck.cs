using InteractionSystem;
using InteractionSystem.Runtime.InteractionConditions;
using UnityEngine;

public class IC_BooleanCheck : InteractionCondition
{
    [SerializeField] private BooleanConditionSO booleanConditionSo;
    [SerializeField] private bool requiredValue;
    
    public override bool isConditionsMet()
    {
        if (booleanConditionSo.Value == requiredValue)
            return true;
        return false;
    }
}
