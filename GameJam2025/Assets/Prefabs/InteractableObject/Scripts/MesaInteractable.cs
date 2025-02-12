using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string action = "Inspeccionar";
    private int id;


    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return action;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetId()
    {
        return id;
    }

    public Description GetDescription()
    {
        return new Description("Mesa", transform, 1f);
    }
}
