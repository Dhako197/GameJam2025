using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Picable : MonoBehaviour, IInteractable
{
    [Header("Description")]
    [SerializeField] private string objectName = string.Empty;
    [SerializeField] private float customNameOffset = 0;

    [Header("FoundItem")]
    [SerializeField] private string itemName = string.Empty;
    [SerializeField] private int foundItemAmount = 0;

    private FoundItem foundItem;
    private int id;

    private void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
        int amout = foundItemAmount != 0 ? foundItemAmount : 1;
        foundItem = new FoundItem(itemName, amout);        ;
    }

    public FoundItem Take()
    {
        gameObject.SetActive(false);
        return foundItem;
    }

    public string GetAction()
    {
        return "Tomar";
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
