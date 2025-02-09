using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    private float life = 60;
    private float baseBubbleTime = 5.0f;
    private float randomRange = 3;
    private float counter = 0;
    private float timeLimit = 0;
    private bool onCooldown = false;
    private float cooldownTime = 5;
    private Animator animator;

    private readonly string takeDamageAction = "takeDamage";
    private readonly string isTrappedAction = "isTrapped";
    private readonly string startGumAction = "startGum";

    void Start()
    {
        UnityEngine.Random.InitState(seed);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        checkResetTimer();

    }

    void checkResetTimer()
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
            animator.SetBool(startGumAction, true);
            counter = 0;
            onCooldown = true;
        }
    }


    void setTimeLimit()
    {
        animator.SetBool(startGumAction, false);
        counter = 0;
        float randomizer = UnityEngine.Random.value;
        bool isAddition = randomizer > 0.5;
        timeLimit = baseBubbleTime + (randomRange * (randomizer) * (isAddition ? 1 : -1));
    }

    public void TakeDamage(float damage)
    {
        if (life <= 0)
        {
            return;
        }

        string action = life - damage > 0 ? takeDamageAction : isTrappedAction;
        life -= damage;
        animator.SetBool(action, true);
    }
}
