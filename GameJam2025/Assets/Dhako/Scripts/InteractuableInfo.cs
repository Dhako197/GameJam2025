using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableInfo : MonoBehaviour,IInteractable
{
    public int InfoId;
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        string actionType = "Info";
        return  actionType;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetId()
    {
        return id;
    }
}
