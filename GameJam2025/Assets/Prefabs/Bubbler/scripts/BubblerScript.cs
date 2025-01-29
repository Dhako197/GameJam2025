using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using static UnityEngine.InputManagerEntry;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private bool requiresIntro;
    [SerializeField] private Transform _transform;

    private readonly string isSittingAnimation = "isSitting";
    private readonly string isTalkingAnimation = "Talk";
    private readonly string forwardMovementAnimation = "forwardMovement";
    private readonly string backwardMovementAnimation = "backwardMovement";

    private Animator animator;
    private TextMeshProUGUI textBoxInteractions;
    private TextMeshProUGUI textBoxComments;
    private TextMeshProUGUI textBoxIntro;
    private bool crouching = false;
    private bool isSitting = false;
    private bool canMove = false;
    private bool canShoot = false;
    public int introCommentCounter = 0;
    private IInteractable currentInteractable;
    private Vector3 initialPosition = Vector3.zero;
    private List<int> interactableIds = new List<int>();
    private List<IInteractable> interactables = new List<IInteractable>();

    [Header("Burbujas de dialogo")]
    [SerializeField] private GameObject bubbleInteractions;
    [SerializeField] private GameObject bubbleComments;
    [SerializeField] private GameObject bubbleTextDinamico;


    [Header("Introduccion")]
    [SerializeField] private string[] introTextos;
    
    private bool isIntroOn = true;

    [Header("Cambio")]
    private bool changeOportunity = false;
    private bool GetKey = false;
    [SerializeField] private GameObject LlavesObj;
    [SerializeField] private GameObject LlavesJuegueteObj;
    
    [Header("NextLEvel")]
    private bool NextLevel = false;
    
    
    [Header("Disparo")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private int cantidadBalas;


    void Start()
    {
        textBoxInteractions = bubbleInteractions.GetComponentInChildren<TextMeshProUGUI>();
        textBoxComments = bubbleComments.GetComponentInChildren<TextMeshProUGUI>();
        textBoxIntro = bubbleTextDinamico.GetComponentInChildren<TextMeshProUGUI>();

        animator = GetComponentInChildren<Animator>();
        canShoot = SceneManager.GetActiveScene().name.Contains("Disparo");
        initialPosition = transform.position;

        if (requiresIntro)
        {
            bubbleTextDinamico.SetActive(true);
            animator.SetBool(isTalkingAnimation, true);
            canMove = false;
            UpdateText();
        }
    }

    void Update()
    {
        Walk();
        Crouch();
        Shoot();
        ExecuteAction();
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null) { return; }
        int id = interactable.GetId();

        if (interactableIds.Contains(id))
        {
            return;
        }

        interactableIds.Add(id);
        interactables.Add(interactable);
        bubbleInteractions.SetActive(true);
        textBoxInteractions.text = BuildActionMessage(interactable.GetAction());

        //if (other.CompareTag("Teacher") && InventarioController.Instance._inventario[1].Cantidad > 0)
        if (other.CompareTag("Teacher"))
        {
            bubbleInteractions.SetActive(true);
            textBoxInteractions.text = new string("Presiona E parta el cambio ninja");
            changeOportunity = true;
        }

        //if (other.CompareTag("Puerta") && InventarioController.Instance._inventario[2].Cantidad > 0)
        if (other.CompareTag("Puerta"))
        {
            bubbleInteractions.SetActive(true);
            textBoxInteractions.text = new string("Presiona E para empezar tu venganza");
            NextLevel = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null) { return; }
        int id = interactable.GetId();


        int deleteIndex = interactableIds.IndexOf(id);
        interactableIds.Remove(id);
        interactables.RemoveAt(deleteIndex);

        if (interactables.Count == 0)
        {
            currentInteractable = null;
            bubbleInteractions.SetActive(false);
        }
        else
        {
            currentInteractable = interactables.Last();
        }

        if (other.CompareTag("Teacher"))
        {
            bubbleTextDinamico.SetActive(false);
            changeOportunity = false;
        }
        if (other.CompareTag("Puerta"))
        {
            bubbleTextDinamico.SetActive(false);
            NextLevel = false;
        }
    }

    void Shoot()
    {
        if (canShoot && Input.GetButtonDown("Shoot"))
        {
            Debug.Log("Disparando...");
            if (cantidadBalas > 0)
            {
                Disparar();
            }
            else
            {
                Debug.Log("No tienes balas...");
            }
        }

    }
    
    void ExecuteAction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (isIntroOn)
            {
                UpdateText();
                return;
            }

            if (interactables.Count != 0)
            {
                currentInteractable = interactables.Last();
                string action = currentInteractable.GetAction();
                GameObject item = currentInteractable.GetObject();

                InteractionExecution(action, item);
                ClearLastInteractable();
            }
        }
    }

    private void InteractionExecution(string action, GameObject item)
    {
        if (action == "Sentarse")
        {
            Sit();
            return;
        }

        if (action == "Inspeccionar")
        {
            bubbleInteractions.SetActive(true);
            animator.SetBool(isTalkingAnimation, true);
            canMove = false;
            Invoke(nameof(OpinionBubble), 1.5f);
            return;
        }

        if (action == "Tomar")
        {
            if (item != null)
            {
                PicableTest picableObj = item.GetComponent<PicableTest>();
                InventarioController.Instance.SetObjectUI(picableObj);
            }
            return;
        }

        if (action == "Info")
        {
            if (item != null)
            {
                InteractuableInfo interactuableInfo = item.GetComponent<InteractuableInfo>();
                InventarioController.Instance.NoCollectionableInfo(interactuableInfo.InfoId);
            }
            return;
        }

        if (action == "Abrir") // meant to replace the NextLevel == true logic
        {
            /*
             if (NextLevel == true)
            {
                currentInteractable.GetObject().GetComponentInChildren<Puerta>().Open();
                InventarioController.Instance._inventario[2].Cantidad--;
                SceneManager.LoadScene("Scenes/Dhako/Test");
            }
             */
            return;
        }

        if (action == "Reemplazar") // meant to replace the changeOportunity == true && GetKey == false logic
        {
            /*
             if (changeOportunity == true && GetKey == false)
            {
                InventarioController.Instance.UseItem(2);
                InventarioController.Instance.SetObByID(3);
                LlavesObj.SetActive(false);
                LlavesJuegueteObj.SetActive(true);
                bubbleTextDinamico.SetActive(false);
                GetKey = true;

            }
            */
            return;
        }
    }

    private void SetCamera()
    {
        //(0, 1.25,-2.49)
        //(0,3,-6)
        GameObject followPlayerGo = GameObject.FindGameObjectWithTag("MainCamera");
        FollowPlayer followPlayerScript = followPlayerGo.GetComponent<FollowPlayer>();
        if (followPlayerScript != null)
        {
            followPlayerScript.offset = new Vector3(0, 3, -6);
        }
    }

    private void UpdateText()
    {
        if (introCommentCounter < introTextos.Length )
        {
            textBoxIntro.text = introTextos[introCommentCounter];
            introCommentCounter++;
        }
        else
        {
            SetCamera();
            bubbleTextDinamico.SetActive(false);
            isIntroOn = false;
            canMove = true;
            animator.SetBool(isTalkingAnimation, false);
        }
       
    }

    void Walk()
    {
        if (!canMove) { return; }

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if (Math.Abs(horizontalMovement) < 0.1 && Math.Abs(verticalMovement) < 0.1)
        {
            animator.SetBool(forwardMovementAnimation, false);
            animator.SetBool(backwardMovementAnimation, false);
            return;
        }

        isSitting = false;
        animator.SetBool(isSittingAnimation, false);
        animator.SetBool(forwardMovementAnimation, true);
        animator.SetBool(backwardMovementAnimation, verticalMovement > 0);

        float localSpeed = crouching ? speed / 2 : speed;
        Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement).normalized * localSpeed * Time.deltaTime;
           
        transform.Translate(movement, Space.Self);

        _transform.localRotation = horizontalMovement < 0 ? new Quaternion(0, 180, 0, 1) : new Quaternion(0, 0, 0, 0);
    }

    void Crouch()
    {
        crouching = Input.GetButton("Crouch");
    }

    void Sit()
    {
        isSitting = true;
        Vector3 seatPosition = currentInteractable.GetObject().transform.position;
        transform.position = new Vector3(seatPosition.x, transform.position.y, seatPosition.z + 0.5f);
        animator.SetBool(isSittingAnimation, true);
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    private void OpinionBubble()
    {
        animator.SetBool(isTalkingAnimation, false);
        bubbleComments.SetActive(false);
        canMove = true;
    }
    
    public void Disparar()
    {
        cantidadBalas--;
        Instantiate(prefabBala, controladorDisparo.position, controladorDisparo.rotation);
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

    public void ClearLastInteractable()
    {
        interactableIds.RemoveAt(interactableIds.Count - 1);
        interactables.RemoveAt(interactables.Count - 1);
        currentInteractable = interactables.Last();

        if (currentInteractable == null)
        {
            bubbleInteractions.SetActive(false);
            return;
        }
        
        textBoxInteractions = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textBoxInteractions.text = BuildActionMessage(currentInteractable.GetAction());
    }

    public void ClearInteractable()
    {
        bubbleComments.SetActive(false);
        bubbleInteractions.SetActive(false);
        interactableIds.Clear();
        interactables.Clear();
        currentInteractable = null;
    }

    public void SetIntroOn(bool intro)
    {

    }
}
