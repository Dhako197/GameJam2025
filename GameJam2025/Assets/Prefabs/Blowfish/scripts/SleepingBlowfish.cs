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
    private bool isSleeping = true;
    private bool isChecking = false;
    private float counter = 0;
    private float sleepLimit = 0;
    private Animator[] animators;
    private Animator animator;
    private Animator bubbleAnimator;

    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BubblerScript>();
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
        //bubbleAnimator.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSleep();
        CheckRoom();
    }

    void CheckRoom()
    {
        if (!isChecking)
        {
            return;
        }

        //bubbleAnimator.GetComponent<SpriteRenderer>().enabled = false;
        if (playerController != null && playerController.IsSitting())
        {
            // increases one hour
            // animation returns player
            //animator.SetBool("isWalking", true);
            //Transform player = GameObject.FindGameObjectWithTag("Player");
            //transform.position = new Vector3(pla)

            reachBubbler();
            playerController.Reset();
            return;
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
            //bubbleAnimator.GetComponent<SpriteRenderer>().enabled = true;

        }
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

    void reachBubbler()
    {

    }
}
