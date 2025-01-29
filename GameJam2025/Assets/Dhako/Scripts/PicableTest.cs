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

    private int _actionId;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        _actionId = System.Guid.NewGuid().GetHashCode();
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
        return _actionId;
    }
    
}
