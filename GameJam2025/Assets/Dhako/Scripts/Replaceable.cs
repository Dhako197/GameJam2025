using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replacable : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [Header("Description")]
    [SerializeField] private string objectName = string.Empty;
    [SerializeField] private float customNameOffset = 0;

    private int id;
    
    void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return "Reemplazar";
    }

    public GameObject GetObject()
    {
        return this.gameObject;
    }

    public int GetId()
    {
        return id;
    }

    public Description GetDescription()
    {
        return new Description(objectName, transform, customNameOffset);
    }
}

