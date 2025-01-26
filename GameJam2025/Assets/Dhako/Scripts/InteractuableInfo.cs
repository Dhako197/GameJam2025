using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableInfo : MonoBehaviour,IInteractable
{
    public int InfoId;

    public string GetAction()
    {
        string actionType = "Info";
        return  actionType;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
