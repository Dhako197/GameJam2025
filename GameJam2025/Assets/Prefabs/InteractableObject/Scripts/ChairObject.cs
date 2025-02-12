using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairObject : MonoBehaviour, IInteractable
{
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
   {
      return "Sentarse";
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
        return new Description("Silla", transform, 0.5f);
    }
}
