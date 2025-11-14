using UnityEngine;

namespace InteractionSystem.Runtime.InteractionConditions
{
    [System.Serializable]
    public abstract class InteractionCondition
    {
        public abstract bool isConditionsMet();
    }
}