using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    private float baseBubbleTime = 5.0f;
    private float randomRange = 3;
    private float counter = 0;
    private float timeLimit = 0;
    private bool onCooldown = false;
    private float cooldownTime = 5;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(seed);
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
        float randomizer = UnityEngine.Random.value;
        bool isAddition = randomizer > 0.5;
        timeLimit = baseBubbleTime + (randomRange * (randomizer) * (isAddition ? 1 : -1));
    }
}
