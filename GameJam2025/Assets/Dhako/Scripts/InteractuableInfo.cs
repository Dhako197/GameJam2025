using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableInfo : MonoBehaviour, IInteractable
{
    public int InfoId;
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return "Info";
    }

    public GameObject GetObject()
    {
        return this.gameObject;
    }

    public int GetId()
    {
        return id;
    }

    public Description GetDescription()
    {
        return new Description("Curiosidad", transform, 0.5f);
    }
}
