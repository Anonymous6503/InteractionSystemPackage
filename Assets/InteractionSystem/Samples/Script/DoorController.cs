using System;
using UnityEngine;

public class DoorController : AnimatorController
{
    [SerializeField] private bool _isOpen;

    public void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("Open", _isOpen);
    }
    
    public void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("Open", _isOpen);
    }
}
