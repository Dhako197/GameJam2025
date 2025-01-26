using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float baseBubbleTime = 10.0f;
    private Animator animator;
    private float counter = 0;
    private float timeLimit = 0;
    private bool onCooldown = false;
    private float cooldownTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (onCooldown)
        {
            if (counter < cooldownTime)
            {
                return;
            }

            onCooldown = false;
            setTimeLimit();
        }

        Debug.Log(counter + " " + timeLimit);
        if (counter > timeLimit)
        {
            animator.SetBool("startGum", true);
            counter = 0;
            onCooldown = true;
        }
    }


    void setTimeLimit()
    {
        animator.SetBool("startGum", false);
        counter = 0;
        timeLimit = baseBubbleTime * (1 + (DateTime.Now.Millisecond /1000));
    }
}
