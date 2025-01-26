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

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
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
        animator.SetBool("isOpen", true);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
    
