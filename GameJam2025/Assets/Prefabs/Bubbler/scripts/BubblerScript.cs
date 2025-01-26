using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Collections.Specialized.BitVector32;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] public float speed = 8f;
    private bool crouching = false;
    private bool isSitting = false;
    private IInteractable currentInteractable;
    private Animator _animator;
    private Vector3 initialPosition = Vector3.zero;
    public bool Stand = true;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _bubbleText;
    [SerializeField] private GameObject _bubbleTextOpinion;

    [Header("Introduccion")]
    private bool isIntroOn = true;
    [SerializeField] private string[] introTextos;
    [SerializeField] private GameObject _bubbleTextDinamico;
    [SerializeField] private TextMeshProUGUI introTMPro;
    public int contador = 0;
    private bool canMove = false;

    [Header("Cambio")]
    private bool changeOportunity = false;
    private bool GetKey = false;
    
    [Header("NextLEvel")]
    private bool NextLevel = false;
    

    [SerializeField] private TextMeshProUGUI textBox;
    
    [Header("Disparo")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private int cantidadBalas;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        _animator = GetComponentInChildren<Animator>();
        _bubbleTextDinamico.SetActive(true);
        canMove = false;
        _animator.SetBool("Talk", true);
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        specialInteractions();
        Walk();
        Crouch();
        ExecuteAction();
    }

    private void specialInteractions()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (isIntroOn)
            {
                contador++;
                UpdateText();
            }

            if (changeOportunity == true && GetKey == false)
            {
                InventarioController.Instance._inventario[1].Cantidad--;
                InventarioController.Instance.SetObByID(3);
                _bubbleTextDinamico.SetActive(false);
                GetKey = true;

            }

            if (NextLevel == true)
            {
                currentInteractable.GetObject().GetComponentInChildren<Puerta>().Open();
                InventarioController.Instance._inventario[2].Cantidad--;
                SceneManager.LoadScene("Scenes/Dhako/Test");
            }

            if (SceneManager.GetActiveScene().name.Contains("Disparo"))
            {
                Debug.Log("Disparando...");
                if (cantidadBalas > 0)
                {
                    Disparar();
                    cantidadBalas--;
                }
                else
                {
                    Debug.Log("No tienes balas...");
                }
            }
        }
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
        if (contador < introTextos.Length )
        {
            introTMPro.text = introTextos[contador];
        }
        else
        {
            Debug.Log("termino intro");
            SetCamera();
            _bubbleTextDinamico.SetActive(false);
            isIntroOn = false;
            canMove = true;
            _animator.SetBool("Talk", false);
        }
       
    }
    void ExecuteAction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (currentInteractable != null)
            {
                _bubbleText.SetActive(false);
                string action = currentInteractable.GetAction();

                if (action == "Sentarse")
                {
                    _bubbleText.SetActive(false);
                    Sit();
                }

                if (action == "Inspeccionar") 
                {
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
                        PicableTest picableObj= item.GetComponent<PicableTest>();
                        InventarioController.Instance.SetObjectUI(picableObj);
                    }
                }

                if (action == "Info")
                {
                    GameObject info = currentInteractable.GetObject();
                    if (info != null)
                    {
                        InteractuableInfo interactuableInfo = info.GetComponent<InteractuableInfo>();
                        InventarioController.Instance.NoCollectionableInfo(interactuableInfo.InfoId);
                    }

                }
            }
        }
    }

    void Walk()
    {
        if (!canMove) { return; }
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

        if (other.CompareTag("Teacher") && InventarioController.Instance._inventario[1].Cantidad > 0)
        {
            _bubbleTextDinamico.SetActive(true);
            introTMPro.text = new string("Presiona E parta el cambio ninja");
            changeOportunity = true;
        }

        if (other.CompareTag("Puerta") && InventarioController.Instance._inventario[2].Cantidad > 0)
        {
            _bubbleTextDinamico.SetActive(true);
            introTMPro.text = new string("Presiona E para empezar tu venganza");
            NextLevel = true;

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
        if (other.CompareTag("Teacher"))
        {
            _bubbleTextDinamico.SetActive(false);
            changeOportunity = false;
        }
        if (other.CompareTag("Puerta"))
        {
            _bubbleTextDinamico.SetActive(false);
            NextLevel = false;
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
    
    public void Disparar()
    {
        Instantiate(prefabBala, controladorDisparo.position, controladorDisparo.rotation);
    }

    public void Reset()
    {

    }

    string BuildActionMessage(string actionName)
    {
        return actionName + ", presiona e";
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }

    public bool GetIsIntroOn()
    {
        return isIntroOn;
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
    }
}
