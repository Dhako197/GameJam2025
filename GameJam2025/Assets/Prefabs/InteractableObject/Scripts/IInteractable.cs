using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetAction();
    void Interact();
    bool CanBePickedUp();
    void PickUp();
} 
