using UnityEngine;
using UnityEngine.Events;

public class InteractionStrategy_Toggle : InteractionStrategy
{
    [SerializeField] private string  _strategyName = "Toggle Interaction";
    
    private bool _isOn = false;

    [SerializeField] private UnityEvent OnToggleOnAction;
    [SerializeField] private UnityEvent OnToggleOffAction;
    
    public override void Execute(GameObject gameObjectToInteractWith)
    {
        if (_isOn)
        {
            Debug.Log("Toggle Onn on gameobject " + gameObjectToInteractWith.name);
            OnToggleOffAction.Invoke();
        }
        else
        {
            Debug.Log("Toggle Onn on gameobject " + gameObjectToInteractWith.name);
            OnToggleOnAction.Invoke();
        }
        _isOn = !_isOn;
    }
}
