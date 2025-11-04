using UnityEngine;

[System.Serializable]
public abstract class InteractionStrategy
{
    public abstract void Execute(GameObject gameObjectToInteractWith);
}
