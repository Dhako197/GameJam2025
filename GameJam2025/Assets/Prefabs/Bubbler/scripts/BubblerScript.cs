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
    [SerializeField] private float sensitivity = 0.25f;
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
    private FollowPlayer followPlayerScript;
    private float opinionCooldownTimer = 0;
    private float opinionCooldownCounter = 0;
    private bool crouching = false;
    private bool isSitting = false;
    private bool isFirstCatch = true;
    private bool isInstructionsTalking = false;
    private bool canMove = true;
    private bool canShoot = false;
    private IInteractable currentInteractable;
    private Vector3 initialPosition = Vector3.zero;
    private readonly List<string> dialogs = new List<string>();
    private readonly List<IInteractable> interactables = new List<IInteractable>();

    [Header("Burbujas de dialogo")]
    [SerializeField] private GameObject bubbleInteractions;
    [SerializeField] private GameObject bubbleComments;
    [SerializeField] private GameObject bubbleTextDinamico;


    [Header("Introduccion")]
    [SerializeField] private string[] introTextos;
    
    [Header("Cambio")]
    [SerializeField] private GameObject LlavesObj;
    [SerializeField] private Sprite replacementKeys;

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
        followPlayerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();

        initialPosition = transform.position;

        if (requiresIntro)
        {
            for (int i = 0; i < introTextos.Length; i++)
            {
                dialogs.Add(introTextos[i]);
            }
        }
    }

    void Update()
    {
        Walk();
        Crouch();
        Shoot();
        Interact();
        StartDialog();
        ClearOpinion();
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null) { return; }
        string action = interactable.GetAction();

        if (action == "Reemplazar")
        {
            bool hasDoorKeys = InventarioController.Instance.HasDoorKeys();
            if (hasDoorKeys) { return; }
        }

        if (action == "Abrir")
        {
            action = InventarioController.Instance.HasDoorKeys() ? "Empezar venganza" : action;
        }

        interactables.Add(interactable);
        bubbleInteractions.SetActive(true);
        textBoxInteractions.text = BuildActionMessage(action);

    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null) { return; }

        interactables.Remove(interactable);

        if (interactables.Count == 0)
        {
            currentInteractable = null;
            bubbleInteractions.SetActive(false);
        }
        else
        {
            currentInteractable = interactables.Last();
        }
    }

    void Shoot()
    {
        if (canShoot && Input.GetButtonDown("Shoot"))
        {
            Debug.Log("Disparando...");
            if (cantidadBalas > 0)
            {
                cantidadBalas--;
                Instantiate(prefabBala, controladorDisparo.position, controladorDisparo.rotation); ;
            }
            else
            {
                Debug.Log("No tienes balas...");
            }
        }

    }

    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (isInstructionsTalking)
            {
                NextDialog();
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

    void InteractionExecution(string action, GameObject item)
    {
        if (action == "Sentarse")
        {
            Sit();
            return;
        }

        if (action == "Inspeccionar")
        {
            followPlayerScript.Shake();
            OpinionBubbleShow(true, "Parece que no hay nada");
            return;
        }

        if (action == "Tomar")
        {
            if (item != null)
            {
                InventarioController.Instance.SetObjectUI(item.GetComponent<PicableTest>());
            }
            return;
        }

        if (action == "Info")
        {
            if (item != null)
            {
                InventarioController.Instance.NoCollectionableInfo(item.GetComponent<InteractuableInfo>());
            }
            return;
        }

        if (action == "Abrir")
        {
            bool canOpen = InventarioController.Instance.HasDoorKeys();
            if (canOpen)
            {
                currentInteractable.GetObject().GetComponentInChildren<Puerta>().Open();
                Invoke(nameof(LoadNextScene), 1); // late load next scene
            }

            OpinionBubbleShow(!canOpen, "Aun no tengo la llave");
            return;
        }

        if (action == "Reemplazar")
        {
            bool canReplace = InventarioController.Instance.HasReplacementKeys();
            if (canReplace)
            {
                InventarioController.Instance.ReplaceKeys();
                LlavesObj.GetComponentInChildren<SpriteRenderer>().sprite = replacementKeys;
            }

            OpinionBubbleShow(!canReplace, "Necesito algo para reemplazar las llavez");
            return;
        }
    }

    void SetCinematicCamera(bool cinematic)
    {
        // cinematic (0, 1.25, -2.49)
        // gameplay  (0, 3, -6)
        Vector3 offset = cinematic ? new Vector3(0, 1.25f, -2.49f) : new Vector3(0, 3, -6);
        followPlayerScript.offset = offset;
    }

    void StartDialog()
    {
        if (dialogs.Count == 0)
        {
            return;
        }

        SetCinematicCamera(true);
        animator.SetBool(isTalkingAnimation, true);
        bubbleTextDinamico.SetActive(true);
        textBoxIntro.text = dialogs.First();
        isInstructionsTalking = true;
    }

    void NextDialog()
    {
        dialogs.RemoveAt(0);
        if (dialogs.Count == 0)
        {
            SetCinematicCamera(false);
            bubbleTextDinamico.SetActive(false);
            animator.SetBool(isTalkingAnimation, false);
            isInstructionsTalking = false;
        }
    }

    void Walk()
    {
        if (!canMove || isInstructionsTalking) { return; }

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if (Math.Abs(horizontalMovement) < sensitivity && Math.Abs(verticalMovement) < sensitivity)
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

    void OpinionBubbleShow(bool enable)
    {
        animator.SetBool(isTalkingAnimation, enable);
        bubbleComments.SetActive(enable);
        SetOpinionCoolDown(enable);
    }

    void OpinionBubbleShow(bool enable, string text)
    {
        animator.SetBool(isTalkingAnimation, enable);
        bubbleComments.SetActive(enable);
        textBoxComments.text = text;
        SetOpinionCoolDown(enable);
    }

    void SetOpinionCoolDown(bool coolDown)
    {
        if (coolDown)
        {
            opinionCooldownTimer = 1.5f;
            opinionCooldownCounter = 0;
        }
    } 

    void ClearOpinion()
    {
        if (opinionCooldownTimer == 0) { return; }
        if (opinionCooldownCounter < opinionCooldownTimer)
        {
            opinionCooldownCounter += Time.deltaTime;
            return;
        }

        opinionCooldownTimer = 0;
        OpinionBubbleShow(false);
    }

    void OpinionBubble()
    {
        animator.SetBool(isTalkingAnimation, false);
        bubbleComments.SetActive(false);
        canMove = true;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Scenes/Dhako/Test");
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    string BuildActionMessage(string actionName)
    {
        return actionName + ", presiona e";
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }

    public bool GetIsInstructionsTalking()
    {
        return isInstructionsTalking;
    }

    public void ClearLastInteractable()
    {
        interactables.RemoveAt(interactables.Count - 1);

        if (interactables.Count == 0)
        {
            bubbleInteractions.SetActive(false);
            return;
        }

        currentInteractable = interactables.Last();
        textBoxInteractions.text = BuildActionMessage(currentInteractable.GetAction());
    }

    public void ClearInteractable()
    {
        bubbleComments.SetActive(false);
        bubbleInteractions.SetActive(false);
        interactables.Clear();
        currentInteractable = null;
        canMove = true;
    }

    public void CatchTutorial()
    {
        if (!isFirstCatch)
        {
            return;
        }

        isFirstCatch = false;
        dialogs.Add("Mr.Pluff no me dejará salir");
        dialogs.Add("debo estar sentado cuando despierte");
    }
}
