using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

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
    [SerializeField] private GameObject _bubbleTextOpinion;

    [Header("Introduccion")]
    private bool isIntroOn = true;
    [SerializeField] private string[] introTextos;
    [SerializeField] private GameObject _bubbleTextDinamico;
    [SerializeField] private TextMeshProUGUI introTMPro;
    private int contador=0;
    private bool canMove = false;

    [SerializeField] private TextMeshProUGUI textBox;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _bubbleTextDinamico.SetActive(true);
        canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIntroOn)
        {
            UpdateText();
            if (Input.GetButton("Interact"))
            {
                contador++;
            }
        }
        
        
        if(canMove) Walk();
        Crouch();
        ExecuteAction();
    }

    private void SetCamera()
    {
        //(0, 1.25,-2.49)
        //(0,3,-6)
        GameObject followPlayerGo= GameObject.FindGameObjectWithTag("MainCamera");
        FollowPlayer followPlayerScriop = followPlayerGo.GetComponent<FollowPlayer>();
        if (followPlayerScriop != null)
        {
            followPlayerScriop.offset = new Vector3(0, 3, -6);
        }

    }

    private void UpdateText()
    {
        if (contador >= introTextos.Length -1)
        {
            introTMPro.text = introTextos[contador];
        }
        else
        {
            SetCamera();
            _bubbleTextDinamico.SetActive(false);
            isIntroOn = false;
            canMove = true;
        }
       
    }


    void ExecuteAction()
    {
        if (Input.GetButton("Interact"))
        {
            if (currentInteractable != null)
            {
                _bubbleText.SetActive(false);
                string action = currentInteractable.GetAction();

                //currentInteractable.Interact(gameObject);
                if (action == "Sentarse")
                {
                    _bubbleText.SetActive(false);
                    Sit();
                }

                if (action == "Inspeccionar") 
                {
                    //GameObject inspectedObject = currentInteractable.GetObject();
                    //Transform inspectedTransform = inspectedObject.transform;
                    // focus camera
                    //GameObject innerElement = inspectedObject.GetComponent<NonCollectibleObject>().GetInnerElement();
                    //currentInteractable = innerElement.GetComponent<IInteractable>();
                    _bubbleTextOpinion.SetActive(true);
                    _animator.SetBool("Talk", true);
                    canMove = false;
                    Invoke("OpinionBubble", 1.5f);
                }

                if (action == "Tomar")
                {
                    GameObject item = currentInteractable.GetObject();
                    if (item != null)
                    {
                        // Add to inventory
                        PicableTest picableObj= item.GetComponent<PicableTest>();
                        InventarioController.Instance.SetObjectUI(picableObj);
                        Destroy(item);
                    }
                   
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
            return;
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
        Vector3 seatPosition = currentInteractable.GetObject().transform.position;
        transform.position = new Vector3(seatPosition.x, transform.position.y, seatPosition.z + 0.5f);
        _animator.SetBool("isSitting", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log(currentInteractable.GetAction());
            _bubbleText.SetActive(true);
            textBox = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            textBox.text = BuildActionMessage(currentInteractable.GetAction());
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

    private void OpinionBubble()
    {
        _animator.SetBool("Talk", false);
        _bubbleTextOpinion.SetActive(false);
        canMove = true;
    }

    public void Reset()
    {

    }

    string BuildActionMessage(string actionName)
    {
        return actionName + ", presiona e";
    }

}
