using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IInteractable
{
    [Header("DoorLogic")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private string phase;

    [Header("Description")]
    [SerializeField] private float customNameOffsetY = 0;
    [SerializeField] private float customNameOffsetX = 0;

    private readonly string objectName = "Puerta";
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
        boxCollider.isTrigger = true;
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

    public bool GetIsOpen()
    {
        return animator.GetBool("isOpen");
    }

    public string GetPhase()
    {
        return phase;
    }

    public Description GetDescription()
    {
        return new Description(objectName, transform, customNameOffsetY, customNameOffsetX);
    }
}
    
