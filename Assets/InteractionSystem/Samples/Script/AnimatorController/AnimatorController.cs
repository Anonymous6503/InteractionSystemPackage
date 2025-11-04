using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Animator))]
public abstract class AnimatorController : MonoBehaviour
{
    public Animator _animator;
    public virtual void Start()
    {
        if(_animator == null)
            _animator = this.GetComponent<Animator>();
    }
}
