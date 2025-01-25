using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PicableTest : MonoBehaviour, IInteractable
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _nombre;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public string GetAction()
    {
        string action = "Recogio " + _nombre;
        return action;
    }

    public GameObject GetObject()
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        InventarioController.Instance.SetObjectUI(_sprite,_id,_nombre);
        Destroy(gameObject);
    }

    public bool CanBePickedUp()
    {
        return false;
    }

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }
}
