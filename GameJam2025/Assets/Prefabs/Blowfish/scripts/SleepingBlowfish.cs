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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BubblerScript>();
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        animator.SetBool("isSleeping", isSleeping);
        animator.SetBool("isChecking", isChecking);

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

        if (playerController != null && playerController.IsSitting())
        {
            // increases one hour
            // animation returns player
            playerController.Reset();
            return;
        }

        SetIsChecking(false);
    }

    public void ResetSleep()
    {
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        SetIsSleeping(true);
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
}
