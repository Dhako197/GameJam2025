using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PicableTest : MonoBehaviour, IInteractable
{
    [SerializeField] public int _id;
    [SerializeField] public Sprite _sprite;
    [SerializeField] public string _nombre;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public string GetAction()
    {
        string action = "Tomar";
        return action;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public void Interact()
    {
       
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
