using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IInteractable
{
    [SerializeField] private Color _color;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxCollider _boxCollider;
    private Animator animator;
    private int id;


    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        id = System.Guid.NewGuid().GetHashCode();
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

    public void Open()
    {
        _spriteRenderer.color = _color;
        _boxCollider.enabled = false;
        animator.SetBool("isOpen", true);
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
    
