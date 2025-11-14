using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Conditions", menuName = "Interacting Conditions/Boolean Condition")]
public class BooleanConditionSO : ScriptableObject
{
    [Tooltip("The value of this particular condition.")]
    public bool Value = false;

    private void OnDisable()
    {
        ResetValue();
    }

    private void ResetValue()
    {
        Value = false;
    }

    public void SetValue(bool value)
    {
        this.Value = value;
    }
}
