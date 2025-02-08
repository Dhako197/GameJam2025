using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SleepingBlowfish : MonoBehaviour
{
    [SerializeField] private float sleepTimeBase = 9.0f;
    [SerializeField] private float speed = 8.0f;

    private BubblerScript playerController;
    private TeacherAudioController teacherAudioController;
    private GameObject player;
    private bool isSleeping = true;
    private bool isChecking = false;
    private bool isCatching = false;
    private float counter = 0;
    private float sleepLimit = 0;
    private Vector3 ogPosition = new Vector3(-3.65f, 1.45f, 9);
    private Vector3 destination = Vector3.zero;
    private string destinationName = string.Empty;
    private Animator[] animators;
    private Animator animator;
    private Animator bubbleAnimator;
    private FollowPlayer sceneCamera;

    private readonly string isSleepingAnimation = "isSleeping";
    private readonly string isCheckingAnimation = "isChecking";
    private readonly string isWithKidAnimation = "isWithKid";
    private readonly string isChargingAnimation = "isCharging";
    private readonly string isWalkingAnimation = "isWalking";

    void Start()
    {
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();

        teacherAudioController = GetComponentInChildren<TeacherAudioController>();
        animators = GetComponentsInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<BubblerScript>();
        sleepLimit = sleepTimeBase + RandomizeSleepTime();
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i].name == "sprite")
            {
                animator = animators[i];
            }

            if (animators[i].name == "bubble")
            {
                bubbleAnimator = animators[i];
            }
        }
        animator.SetBool(isSleepingAnimation, isSleeping);
        animator.SetBool(isCheckingAnimation, isChecking);
        teacherAudioController.PlaySleeping();
    }

    void Update()
    {
        MoveToDestination();
        CheckSleep();
        CheckRoom();
    }

    void CheckRoom()
    {
        if (!isChecking || isCatching || isSleeping || playerController.GetIsInstructionsTalking())
        {
            return;
        }

        if (playerController != null && !playerController.IsSitting())
        {
            ReachBubbler();
            teacherAudioController.PlayTalking();
            return;
        }
    }

    void MoveToDestination()
    {
        if (destination != Vector3.zero)
        {
            if (destinationName == "kid")
            {
                CatchingKidChanges();
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), Time.deltaTime * speed);
            Vector3 diff = destination - transform.position;

            // proximity when returning behind desk is smaller than catching player
            float proximity = destinationName == String.Empty ? 0.5f : 1.5f;

            if (Math.Abs(diff.x) < proximity && Math.Abs(diff.z) < proximity)
            {
                if (destinationName == "kid") {
                    destinationName = "initialChair";
                    animator.SetBool(isWithKidAnimation, true);
                    sceneCamera.ChangeTransform(transform);
                    playerController.ClearInteractable();
                    destination = playerController.GetInitialPosition();
                    player.SetActive(false);
                    return;
                }

                if (destinationName == "initialChair")
                {
                    player.SetActive(true);
                    sceneCamera.ResetTransform();
                    animator.SetBool(isWithKidAnimation, false);
                    playerController.CatchTutorial();
                    player.transform.position = transform.position;
                    destination = ogPosition;
                    destinationName = String.Empty;
                    return;
                }

                if (destinationName == String.Empty)
                {
                    SetIsSleeping(true);
                    SetIsChecking(false);
                    SetIsWalking(false);
                    destination = Vector3.zero;
                    return;
                }
            }
        }
    }

    void CatchingKidChanges()
    {
        if (!playerController.IsSitting())
        {
            destination = player.transform.position;
            return;
        }

        destination = ogPosition;
        destinationName = String.Empty;
    }

    public void ResetSleep()
    {
        sleepLimit = sleepTimeBase + RandomizeSleepTime();
        SetIsSleeping(true);
        teacherAudioController.PlaySleeping();
        bubbleAnimator.SetBool(isChargingAnimation, false);
        counter = 0;
    }

    void CheckSleep()
    {
        if (!isSleeping)
        {
            return;
        }

        counter += Time.deltaTime;
        if (counter > sleepLimit) {
            SetIsSleeping(false);
            teacherAudioController.PlayBlop();
            bubbleAnimator.SetBool(isChargingAnimation, true);
        }
    }

    void ReachBubbler()
    {
        SetIsChecking(false);
        SetIsWalking(true);

        destination = player.transform.position;
        destinationName = "kid";
    }

    float RandomizeSleepTime()
    {
        float upDownValue = UnityEngine.Random.Range(0, 1) > 0.5 ? 1 : -1;
        float randomizer = (float)Math.Ceiling(UnityEngine.Random.Range(0.0f, sleepTimeBase * 0.2F));
        return randomizer * upDownValue;
    }

    public void SetIsSleeping(bool sleep)
    {
        isSleeping = sleep;
        animator.SetBool(isSleepingAnimation, sleep);
    }

    public void SetIsChecking(bool checking)
    {
        isChecking = checking;
        animator.SetBool(isCheckingAnimation, checking);
    }

    public void SetIsWalking(bool checking)
    {
        isCatching = checking;
        animator.SetBool(isWalkingAnimation, checking);
    }
}
