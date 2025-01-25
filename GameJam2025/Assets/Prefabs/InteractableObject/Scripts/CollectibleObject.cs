using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    public string objectName = "Objeto"; 

    public string GetAction()
    {
        return CanBePickedUp() ? "Coger" : "Interactuar"; 
    }

    public bool CanBePickedUp()
    {
        return true; 
    }

    public void Interact()
    {
        Debug.Log($"Interactuando con {objectName}");
    }

    public void PickUp()
    {
        Debug.Log($"Has recogido {objectName}");
        
        Destroy(gameObject); 
    }
}