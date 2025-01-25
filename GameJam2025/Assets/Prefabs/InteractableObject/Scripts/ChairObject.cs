using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairObject : MonoBehaviour, IInteractable
{
   public string GetAction()
   {
      return "Sentarse";
   }
   
   public bool CanBePickedUp()
   {
      return false;
   }
   
   public void Interact()
   {
      Debug.Log("Sentandose en la silla");
   }
   
   public void PickUp()
   {
      Debug.Log("No se puede recoger la silla");
   }
}
