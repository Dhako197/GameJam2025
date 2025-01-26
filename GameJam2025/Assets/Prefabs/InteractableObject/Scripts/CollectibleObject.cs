using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    public string GetAction()
    {
        return "Tomar"; 
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}