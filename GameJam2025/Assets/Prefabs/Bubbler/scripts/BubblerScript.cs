using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] public float speed = 8f;
    private bool crouching = false;
    private bool isSitting = false;
    private IInteractable currentInteractable;
    private Animator _animator;
    public bool Stand = true;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _bubbleText;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Crouch();
        ExecuteAction();
    }


    void ExecuteAction()
    {
        if (Input.GetKeyDown("Interact"))
        {
            if (currentInteractable != null)
            {
                _bubbleText.SetActive(false);
                string action = currentInteractable.GetAction();

                //currentInteractable.Interact(gameObject);
                if (action == "Sit")
                {
                    Sit();
                }

                if (action == "Inspect") 
                {
                    GameObject inspectedObject = currentInteractable.GetObject();
                    Transform inspectedTransform = inspectedObject.transform;
                    // focus camera
                    GameObject innerElement = inspectedObject.GetComponent<NonCollectibleObject>().GetInnerElement();
                    currentInteractable = innerElement.GetComponent<IInteractable>();
                    _bubbleText.SetActive(true);
                }

                if (action == "Take")
                {
                    GameObject item = currentInteractable.GetObject();
                    // Add to inventory
                    Destroy(item);
                }
            }
        }
    }

    void Walk()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if(Math.Abs((double)horizontalMovement) < 0.1 && Math.Abs((double)verticalMovement) < 0.1)
        {
            _animator.SetBool("forwardMovement", false);
            _animator.SetBool("backwardMovement", false);
            return
        }

        isSitting = false;
        _animator.SetBool("isSitting", false);
        _animator.SetBool("forwardMovement", true);
        _animator.SetBool("backwardMovement", verticalMovement > 0);

        float localSpeed = crouching ? speed / 2 : speed;
        Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement).normalized * localSpeed * Time.deltaTime;
           
        transform.Translate(movement, Space.Self);

        _transform.localRotation = horizontalMovement < 0 ? new Quaternion(0, 180, 0, 1) : new Quaternion(0, 0, 0, 0);
    }

    void Crouch()
    {
        crouching = Input.GetButton("Crouch");
    }

    public void Sit()
    {
        isSitting = true;
        _animator.SetBool("isSitting", true);
        Debug.Log("Sentandose en la silla");
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            _bubbleText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null; 
            _bubbleText.SetActive(false);
        }
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    public void Reset()
    {

    }

}
