using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SleepingBlowfish : MonoBehaviour
{
    [SerializeField] private float sleepTimeBase = 10.0f;
    private bool isSleeping = true;
    private float counter = 0;
    private float sleepLimit = 0;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        animator = GetComponentInChildren<Animator>();
        setIsSleeping(isSleeping);
    }

    // Update is called once per frame
    void Update()
    {
        CheckRoom();
        CheckSleep();
    }

    void CheckRoom()
    {
        if (isSleeping)
        {
            return;
        }

        // check room
        sleepLimit = sleepTimeBase + randomizeSleepTime();
        setIsSleeping(false);
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
            setIsSleeping(false);
        }
    }

    float randomizeSleepTime()
    {
        float upDownValue = UnityEngine.Random.Range(0, 1) > 0.5 ? 1 : -1;
        float randomizer = (float)Math.Ceiling(UnityEngine.Random.Range(0.0f, sleepTimeBase * 0.2F));
        return randomizer * upDownValue;
    }

    void setIsChecking(bool isChecking)
    {
        animator.SetBool("isChecking", isChecking);
    }

    void setIsSleeping(bool sleep)
    {
        isSleeping = sleep;
        animator.SetBool("isSleeping", sleep);
        animator.SetBool("isChecking", !sleep);
    }
}
