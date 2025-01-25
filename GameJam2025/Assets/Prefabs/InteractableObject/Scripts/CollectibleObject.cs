using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    public string GetAction()
    {
        return "Take"; 
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}