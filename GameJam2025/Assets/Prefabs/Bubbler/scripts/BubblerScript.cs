using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    private bool crouching = false;
    private IInteractable currentInteractable;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Crouch();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                Debug.Log($"Interactuando con {currentInteractable.GetAction()}");
                currentInteractable.Interact();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentInteractable != null && currentInteractable.CanBePickedUp())
            {
                Debug.Log($"Has recogido el objeto: {currentInteractable}");
                currentInteractable.PickUp();
            }
        }
    }


    void Inspect()
    {
    }

    void Walk()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement) * speed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
        }
    }

    void Crouch()
    {
        crouching = Input.GetButton("Crouch");
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log($"Objeto interactuable detectado: {interactable.GetAction()}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null; 
            Debug.Log("Salimos de la zona interactuable");
        }
    }

    void CreateBubble()
    {
        
    }


}
