using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairObject : MonoBehaviour, IInteractable
{
   public string GetAction()
   {
      return "Sentarse";
   }
   
    public GameObject GetObject()
    {
        return gameObject;
    }
}
