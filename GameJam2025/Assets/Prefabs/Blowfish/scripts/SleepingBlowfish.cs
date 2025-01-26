using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class SleepingBlowfish : MonoBehaviour
{
    [SerializeField] private float sleepTimeBase = 10.0f;
    private BubblerScript playerController;
    private GameObject player;
    private bool isSleeping = true;
    private bool isChecking = false;
    private float counter = 0;
    private float sleepLimit = 0;
    private Vector3 ogPosition = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private string destinationName = string.Empty;
    private bool isCatching = false;
    private Animator[] animators;
    private Animator animator;
    private Animator bubbleAnimator;


    // Start is called before the first frame update
    void Start()
    {
        ogPosition = transform.position;
        animators = GetComponentsInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<BubblerScript>();
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        for (int i = 0; i < animators.Length; i++)
        {
            Debug.Log(animators[i].name);

            if (animators[i].name == "sprite")
            {
                animator = animators[i];
            }

            if (animators[i].name == "bubble")
            {
                bubbleAnimator = animators[i];
            }
        }
        animator.SetBool("isSleeping", isSleeping);
        animator.SetBool("isChecking", isChecking);
    }

    // Update is called once per frame
    void Update()
    {
        moveToDestination();
        CheckSleep();
        CheckRoom();
    }

    void CheckRoom()
    {
        if (!isChecking || isCatching || isSleeping || playerController.GetIsIntroOn())
        {
            return;
        }

        if (playerController != null && !playerController.IsSitting())
        {
            reachBubbler();
            return;
        }
    }

    void moveToDestination()
    {
        if (destination != Vector3.zero)
        {
            if (destinationName == "Kid")
            {
                destination = GameObject.FindGameObjectWithTag("Player").transform.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), Time.deltaTime * 5);
            Vector3 diff = destination - transform.position;


            if (Math.Abs(diff.x) < 1 && Math.Abs(diff.z) < 1)
            {
                Debug.Log(destinationName);
                if (destinationName == "kid") {
                    animator.SetBool("isWithKid", true);
                    destinationName = "initialChair";
                    player.SetActive(false);
                    FollowPlayer camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
                    camera.ChangeTransform(transform);
                    destination = playerController.GetInitialPosition();
                    Debug.Log(destination);
                    Debug.Log(destinationName);
                    return;
                }

                if (destinationName == "initialChair")
                {
                    player.SetActive(true);
                    player.transform.position = transform.position;
                    FollowPlayer camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
                    camera.ResetTransform();
                    destination = ogPosition;
                    destinationName = String.Empty;
                    animator.SetBool("isWithKid", false);
                    return;
                }

                if (destinationName == String.Empty)
                {
                    SetIsChecking(false);
                    SetIsSleeping(true);
                    SetIsWalking(false);
                    destination = Vector3.zero;
                    return;
                }
            }
        }
    }

    public void ResetSleep()
    {
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        SetIsSleeping(true);
        bubbleAnimator.SetBool("isCharging", false);
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
            bubbleAnimator.SetBool("isCharging", true);
        }
    }

    void reachBubbler()
    {
        Debug.Log("Reaching buibbler");
        SetIsChecking(false);
        SetIsWalking(true);

        destination = player.transform.position;
        destinationName = "kid";
    }

    float randomizeSleepTime()
    {
        float upDownValue = UnityEngine.Random.Range(0, 1) > 0.5 ? 1 : -1;
        float randomizer = (float)Math.Ceiling(UnityEngine.Random.Range(0.0f, sleepTimeBase * 0.2F));
        return randomizer * upDownValue;
    }

    public void SetIsSleeping(bool sleep)
    {
        isSleeping = sleep;
        animator.SetBool("isSleeping", sleep);
    }

    public void SetIsChecking(bool checking)
    {
        isChecking = checking;
        animator.SetBool("isChecking", checking);
    }

    public void SetIsWalking(bool checking)
    {
        isCatching = checking;
        animator.SetBool("isWalking", checking);
    }
}
