using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float sensitivity = 0.4f;
    [SerializeField] private float fadeOutTime = 5f;
    [SerializeField] private int requiredGums = 6;
    [SerializeField] private bool requiresIntro;
    [SerializeField] private Transform _transform;
    [SerializeField] private CanvasGroup blackout;

    private readonly string isSittingAnimation = "isSitting";
    private readonly string isTalkingAnimation = "Talk";
    private readonly string forwardMovementAnimation = "forwardMovement";
    private readonly string backwardMovementAnimation = "backwardMovement";
    private readonly string hasBuiltStrawAnimation = "hasBuiltStraw";
    private readonly string isShootingAnimation = "isShooting";

    private AudioSource audioController;
    private Animator animator;
    private BubblerSoundController bubblerSoundcontroller;
    private TextMeshProUGUI textBoxInteractions;
    private TextMeshProUGUI textBoxComments;
    private TextMeshProUGUI textBoxIntro;
    private RectTransform descriptorCanvas;
    private TextMeshProUGUI descriptor;
    private FollowPlayer followPlayerScript;
    private Rigidbody rb;

    private float opinionCooldownTimer = 0;
    private float opinionCooldownCounter = 0;
    private float endrunCounter = 0;
    private bool endRun = false;
    private bool movementCoolDown = false;
    private bool crouching = false;
    private bool isSitting = false;
    private bool isFirstCatch = true;
    private bool isInstructionsTalking = false;
    private bool canMove = true;
    private bool canShoot = false;
    private bool isSecondFase = false;
    private bool hasBuiltStraw = false;
    private float prevMovement = 1;
    private string nextScene = "Credits";
    private IInteractable currentInteractable;
    private Vector3 initialPosition = Vector3.zero;
    private readonly float fixedSittingYOffset = 1.15f;
    private readonly List<string> dialogs = new List<string>();
    private readonly List<IInteractable> interactables = new List<IInteractable>();

    [Header("Audio")]
    [SerializeField] private AudioClip addItemClip;
    [SerializeField] private AudioClip openDoorClip;
    [SerializeField] private AudioClip takeKeysClip;

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
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject prefabBala;

    private void Awake()
    {
        bubblerSoundcontroller = GetComponentInChildren<BubblerSoundController>();
        textBoxInteractions = bubbleInteractions.GetComponentInChildren<TextMeshProUGUI>();
        textBoxComments = bubbleComments.GetComponentInChildren<TextMeshProUGUI>();
        textBoxIntro = bubbleTextDinamico.GetComponentInChildren<TextMeshProUGUI>();
        audioController = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        audioController.loop = false;
    }

    void Start()
    {   
        followPlayerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
        descriptor = GameObject.FindGameObjectWithTag("Descriptor").GetComponent<TextMeshProUGUI>();
        descriptorCanvas = descriptor.GetComponentInParent<RectTransform>();

        descriptor.enabled = false;
        initialPosition = transform.position;

        if (requiresIntro)
        {
            for (int i = 0; i < introTextos.Length; i++)
            {
                dialogs.Add(introTextos[i]);
            }
            animator.SetBool(isTalkingAnimation, true);
        }
    }

    void Update()
    {
        if (movementCoolDown)
        {
            return;
        }

        Walk();
        Crouch();
        Shoot();
        Interact();
        StartDialog();
        ClearOpinion();
        CheckHasBuiltStraw();
        CheckEndRun();
    }

    void OnTriggerEnter(Collider other)
    {
        if (movementCoolDown) { return; }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable == null) { return; }

        Description description = interactable.GetDescription();
        DisplayDescriptor(description);
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

        if (!interactables.Contains(interactable)) {
            interactables.Add(interactable);
            bubbleInteractions.SetActive(true);
            textBoxInteractions.text = BuildActionMessage(action);
        }

        if (other.CompareTag("Puerta"))
        {
            Puerta p = other.GetComponent<Puerta>();

            string phase = p.GetPhase();

            DisplayDescriptor(description);

            if (phase == "second" && !isSecondFase)
            {
                bool isOpen = p.GetIsOpen();
                if (isOpen)
                {
                    isSecondFase = true;
                }
                return;
            }

            if (phase == "final" && !canShoot)
            {
                bool hasEnoughGums = InventarioController.Instance.GetGumAmount() >= requiredGums;
                if (hasEnoughGums)
                {
                    SetHasBuiltStraw();
                }
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null) { return; }

        interactables.Remove(interactable);

        if (interactables.Count == 0)
        {
            descriptor.enabled = false;
            currentInteractable = null;
            bubbleInteractions.SetActive(false);
            return;
        }

        currentInteractable = interactables.Last();
    }

    void Shoot()
    {
        if (canShoot && Input.GetButtonDown("Shoot"))
        {
            int bullets = InventarioController.Instance.GetGumAmount();
            if (bullets > 0)
            {
                InventarioController.Instance.UseBullet();
                movementCoolDown = true;
                animator.SetBool(isShootingAnimation, true);
            }
            else
            {
                EndRun("PerdisteBullets");
            }
        }

    }

    void Interact()
    {
        if (Input.GetButtonDown("Pass")&& isInstructionsTalking)
        {
            NextDialog();
            return;
        }
        if (Input.GetButtonDown("Interact")&& !isInstructionsTalking)
        {

            if (interactables.Count != 0)
            {
                currentInteractable = interactables.Last();
                string action = currentInteractable.GetAction();
                GameObject interactable = currentInteractable.GetObject();
                if (interactable == null) { return; }
                if (interactable == null) { return; }

                InteractionExecution(action, interactable);

                if (action != "Sentarse")
                {
                    ClearLastInteractable();
                    return;
                }

            }
        }
    }

    void InteractionExecution(string action, GameObject interactable)
    {
        if (action == "Sentarse")
        {
            Sit();
            return;
        }

        if (action == "Inspeccionar")
        {
            followPlayerScript.Shake();
            Inspectable inspectable = interactable.GetComponent<Inspectable>();

            if (inspectable == null)
            {
                OpinionBubbleShow(true, "Parece que no hay nada");
                return;
            }

            FoundItem foundItem = inspectable.Inspect();

            if (foundItem == null)
            {
                OpinionBubbleShow(true, "Parece que no hay nada");
                return;
            }

            InventarioController.Instance.AddItem(foundItem);
            return;
        }

        if (action == "Tomar")
        {
            if (interactable != null)
            {
                followPlayerScript.Shake();
                Picable inspectable = interactable.GetComponent<Picable>();
                FoundItem foundItem = inspectable.Take();

                PlaySound(addItemClip);
                InventarioController.Instance.AddItem(foundItem);
            }
            return;
        }

        if (action == "Reemplazar")
        {
            bool canReplace = InventarioController.Instance.HasReplacementKeys();
            if (canReplace)
            {
                followPlayerScript.Shake();
                PlaySound(takeKeysClip);
                InventarioController.Instance.ReplaceKeys();
                LlavesObj.GetComponentInChildren<SpriteRenderer>().sprite = replacementKeys;
                return;
            }

            OpinionBubbleShow(!canReplace, "Necesito algo para reemplazar las llavez");
            return;
        }

        if (action == "Info")
        {
            if (interactable != null)
            {
                InventarioController.Instance.NoCollectionableInfo(interactable.GetComponent<InteractuableInfo>());
            }
            return;
        }

        if (action == "Abrir")
        {
            Puerta p = interactable.GetComponent<Puerta>();
            string phase = p.GetPhase();

            bool canOpen = false;
            string errorMessage = "";

            if (phase == "second")
            {
                canOpen = InventarioController.Instance.HasDoorKeys();
                errorMessage = "Aun no tengo la llave";
            }

            if (phase == "final")
            {
                bool hasEnoughGums = InventarioController.Instance.GetGumAmount() >= requiredGums;
                canOpen = hasEnoughGums && InventarioController.Instance.HasStraw();
                errorMessage = "Necesito chicles y un pitillo para mi venganza";
            }

            if (canOpen)
            {
                PlaySound(openDoorClip);
                p.Open();
                return;
            }

            OpinionBubbleShow(!canOpen, errorMessage);
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
        bubbleTextDinamico.SetActive(true);
        textBoxIntro.text = dialogs.First();
        isInstructionsTalking = true;
    }

    void NextDialog()
    {
        if (dialogs.Count != 0)
        {
            dialogs.RemoveAt(0);
        }

        if (dialogs.Count != 0)
        {
            animator.SetBool(isTalkingAnimation, true);
            return;
        }

        SetCinematicCamera(false);
        bubbleTextDinamico.SetActive(false);
        animator.SetBool(isTalkingAnimation, false);
        isInstructionsTalking = false;
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
        UnFreezeY();
        bubblerSoundcontroller.PlayWalking();
        animator.SetBool(isTalkingAnimation, false);
        animator.SetBool(isSittingAnimation, false);
        animator.SetBool(forwardMovementAnimation, true);
        animator.SetBool(backwardMovementAnimation, verticalMovement > 0);

        float currentMovement = horizontalMovement > 0 ? 1 : -1;
        float localSpeed = crouching ? speed / 2 : speed;
        Vector3 movement = localSpeed * Time.deltaTime * new Vector3(horizontalMovement, 0, verticalMovement).normalized;

        transform.Translate(movement, Space.Self);

        if (prevMovement != currentMovement)
        {
            _transform.Rotate(new Vector3(0, 180, 0), Space.Self);
            prevMovement = currentMovement;
        }
    }

    void Crouch()
    {
        crouching = Input.GetButton("Crouch");
    }

    void Sit()
    {
        isSitting = true;
        Vector3 seatPosition = currentInteractable.GetObject().transform.position;
        transform.position = new Vector3(seatPosition.x, fixedSittingYOffset, seatPosition.z + 0.35f);
        animator.SetBool(isSittingAnimation, true);
        FreezeY();
        ClearInteractable();
        StartTimedCooldown();
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

    void CheckHasBuiltStraw()
    {
        bool builtStraw = animator.GetBool(hasBuiltStrawAnimation);
        // one way conditional, therefore harcoded bool param
        if (hasBuiltStraw != builtStraw)
        {
            animator.SetBool(hasBuiltStrawAnimation, true);
        }
    }

    void CheckEndRun()
    {
        if (!endRun)
        {
            return;
        }

        endrunCounter -= Time.deltaTime;
        blackout.alpha = Mathf.Clamp01(1 - (endrunCounter / fadeOutTime));
        AudioController.Instance.SetVolume(0.65f - (endrunCounter / fadeOutTime));

        if (endrunCounter < 0)
        {
            SceneManager.LoadScene(nextScene);
            AudioController.Instance.SetVolume();
        }
    }

    void SetHasBuiltStraw()
    {
        OpinionBubbleShow(true, "Ahora tengo un servatana" + "\n[Q]");
        hasBuiltStraw = true;
        canShoot = true;
    }

    void StartTimedCooldown()
    {
        movementCoolDown = true;
        Invoke("FinishCooldown", 1f);
    }

    void StartTimedCooldown(float time)
    {
        movementCoolDown = true;
        Invoke("FinishCooldown", time);
    }

    void PlaySound(AudioClip clip)
    {
        audioController.clip = clip;
        audioController.Play();
    }

     public void EndRun(string scene)
    {
        if (endRun)
        {
            return;
        }

        endRun = true;
        endrunCounter = fadeOutTime;
        blackout.gameObject.SetActive(true);
        nextScene = scene;
    }

    void FreezeY()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void UnFreezeY()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void DisplayDescriptor(Description description)
    {
        descriptor.text = description.description;
        Transform target = description.transform;
        Vector3 newPosition = new Vector3(target.position.x + description.offsetX, target.position.y + description.offsetY, target.position.z);
        descriptor.transform.position = newPosition;
        descriptor.enabled = true;
    }

    public bool IsSitting()
    {
        return isSitting;
    }

    public bool IsInSecondFase()
    {
        return isSecondFase;
    }

    string BuildActionMessage(string actionName)
    {
        return actionName + "\n[E]";
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
            descriptor.enabled = false;
            bubbleInteractions.SetActive(false);
            return;
        }

        currentInteractable = interactables.Last();
        DisplayDescriptor(currentInteractable.GetDescription());
        textBoxInteractions.text = BuildActionMessage(currentInteractable.GetAction());
    }

    public void ClearInteractable()
    {
        bubbleComments.SetActive(false);
        bubbleInteractions.SetActive(false);
        interactables.Clear();
        currentInteractable = null;
        descriptor.enabled = false;
        canMove = true;
    }

    public void CatchTutorial()
    {
        if (!isFirstCatch)
        {
            return;
        }

        isFirstCatch = false;
        TutorialController.Instance.TimeTutorialActivate();
        dialogs.Add("");
        dialogs.Add("Mr.Pluff no me deja salir");
        dialogs.Add("debo estar sentado cuando despierte");
    }

    public void ThrowBullet()
    {
        Instantiate(prefabBala, bulletSpawn.position, bulletSpawn.rotation);
    }

    public void FinishCooldown()
    {
        animator.SetBool(isShootingAnimation, false);
        movementCoolDown = false;
    }
}
