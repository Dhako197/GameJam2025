using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IInteractable
{
    [SerializeField] private Color _color;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxCollider _boxCollider;

void Start()
{
    _boxCollider = GetComponent<BoxCollider>();
}
    // Update is called once per frame
    void Update()
    {

    }

    public string GetAction()
    {
        string action = "Abrir Puerta";
        return action;
    }

    public void Interact()
    {
        _spriteRenderer.color = _color;
        _boxCollider.enabled = false;
    }

    public bool CanBePickedUp()
    {
        return false;
    }

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
    
