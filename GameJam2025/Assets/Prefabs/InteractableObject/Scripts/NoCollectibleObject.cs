using UnityEngine;

public class NonCollectibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject innerElement;
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return "Inspeccionar"; 
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public GameObject GetInnerElement()
    {
        return innerElement;
    }
    public int GetId()
    {
        return id;
    }

    public Description GetDescription()
    {
        return new Description("IDK", transform);
    }
}