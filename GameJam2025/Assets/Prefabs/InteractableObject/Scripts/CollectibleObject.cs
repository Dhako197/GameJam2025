using UnityEngine;

public class CollectibleObject : MonoBehaviour, IInteractable
{
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return "Tomar"; 
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