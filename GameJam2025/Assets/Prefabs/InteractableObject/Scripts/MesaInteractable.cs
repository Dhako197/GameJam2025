using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaInteractable : MonoBehaviour, IInteractable
{
      private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetAction()
    {
        string action = "Inspeccionar";
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
}
