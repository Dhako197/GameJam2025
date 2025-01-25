using UnityEngine;

public class NonCollectibleObject : MonoBehaviour, IInteractable
{
    public string objectName = "Objeto";

    public string GetAction()
    {
        return "Interactuar"; 
    }

    public bool CanBePickedUp()
    {
        return false; 
    }

    public void Interact()
    {
        Debug.Log($"Interactuando con {objectName}");
    }

    public void PickUp()
    {
        Debug.Log($"{objectName} no puede ser recogido.");
    }
}