using UnityEngine;

public class NonCollectibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject innerElement;

    public string GetAction()
    {
        return "Inspect"; 
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public GameObject GetInnerElement()
    {
        return innerElement;
    }
}