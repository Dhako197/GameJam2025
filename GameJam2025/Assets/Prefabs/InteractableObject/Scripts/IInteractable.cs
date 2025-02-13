using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetAction();
    GameObject GetObject();
    Description GetDescription();
    int GetId();
} 
