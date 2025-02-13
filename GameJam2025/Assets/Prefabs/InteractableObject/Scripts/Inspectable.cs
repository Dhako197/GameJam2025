using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : MonoBehaviour, IInteractable
{
    [Header("Description")]
    [SerializeField] private string objectName = string.Empty;
    [SerializeField] private float customNameOffset = 0;

    [Header("FoundItem")]
    [SerializeField] private bool ContainsItem = false;
    [SerializeField] private string foundItemName = string.Empty;
    [SerializeField] private int foundItemAmount = 0;

    private FoundItem foundItem = null;
    private int id;

    public void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
        
        if (ContainsItem)
        {
            int amout = foundItemAmount != 0 ? foundItemAmount : 1; 
            foundItem = new FoundItem(foundItemName, amout);
        }
    }

    public FoundItem Inspect()
    {
        FoundItem itemToReturn = foundItem;
        foundItem = null;
        return itemToReturn;
    }

    public string GetAction()
    {
        return "Inspeccionar";
    }

    public GameObject GetObject()
    {
        return this.gameObject;
    }

    public Description GetDescription()
    {
        return new Description(objectName, gameObject.transform, customNameOffset);
    }

    public int GetId()
    {
        return id;
    }
}
