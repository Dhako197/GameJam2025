using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IInteractable
{
    [SerializeField] private Color _color;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxCollider boxCollider;
    private Animator animator;
    private int id;


    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        id = System.Guid.NewGuid().GetHashCode();
    }

    public string GetAction()
    {
        return "Abrir";
    }

    public void Open()
    {
        _spriteRenderer.color = _color;
        boxCollider.enabled = false;
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
    
