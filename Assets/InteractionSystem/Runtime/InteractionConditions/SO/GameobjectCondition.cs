using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Conditions", menuName = "Interacting Conditions/Require Game Object")]
public class GameObjectCondition : ScriptableObject
{
    [Tooltip("The value of this particular condition.")]
    public GameObject gameObject = null;

    private void OnDisable()
    {
        ResetValue();
    }

    private void ResetValue()
    {
        gameObject = null;
    }

    public void SetValue(GameObject value)
    {
        gameObject = value;
    }
}