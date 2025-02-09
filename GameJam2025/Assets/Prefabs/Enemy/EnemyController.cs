using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    private float life = 60;
    private float counter = 0;
    private float timeLimit = 0;
    private bool onCooldown = false;
    private bool needsReposition = false;
    private Vector3 destination = Vector3.zero;
    private readonly float randomRange = 3;
    private readonly float baseBubbleTime = 5.0f;
    private readonly float cooldownTime = 5;
    private readonly float movementRatio = 8;
    private readonly float speed = 3f;
    private Animator animator;

    private readonly string takeDamageAction = "takeDamage";
    private readonly string isTrappedAction = "isTrapped";
    private readonly string startGumAction = "startGum";
    private readonly string repositionAction = "reposition";

    void Start()
    {
        UnityEngine.Random.InitState(seed);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        counter += Time.deltaTime;

        CheckResetBubbleTimer();
        CheckReposition();
        CheckRepositionChange();
    }

    void CheckResetBubbleTimer()
    {
        if (onCooldown)
        {
            if (counter < cooldownTime)
            {
                return;
            }

            onCooldown = false;
            SetBubbleTimeLimit();
        }

        if (counter > timeLimit)
        {
            animator.SetBool(startGumAction, true);
            counter = 0;
            onCooldown = true;
        }
    }


    void SetBubbleTimeLimit()
    {
        animator.SetBool(startGumAction, false);
        counter = 0;
        float randomizer = UnityEngine.Random.value;
        bool isAddition = randomizer > 0.5;
        timeLimit = baseBubbleTime + (randomRange * (randomizer) * (isAddition ? 1 : -1));
    }

    void CheckReposition()
    {
        if (!needsReposition)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), Time.deltaTime * speed);
        Vector3 diff = destination - transform.position;

        if (Math.Abs(diff.x) < 0.5f && Math.Abs(diff.z) < 0.5f)
        {
            if (life == 20)
            {
                destination = GetDestinationRandom(6);
                return;
            }

            animator.SetBool(repositionAction, false);
        }
    }

    void CheckRepositionChange()
    {
        bool isRepositioning = animator.GetBool(repositionAction);
        if (isRepositioning == needsReposition)
        {
            return;
        }

        needsReposition = isRepositioning;
        destination = GetDestinationRandom();
    }

    Vector3 GetDestinationRandom()
    {
        Vector2 destinationRandomizer = UnityEngine.Random.insideUnitCircle * movementRatio;
        float minimumDistance = movementRatio / 2;
        float x = destinationRandomizer.x < minimumDistance ? minimumDistance : destinationRandomizer.x;
        float z = destinationRandomizer.y < minimumDistance ? minimumDistance : destinationRandomizer.y;
        return new Vector3(x, 0, z);
    }

    Vector3 GetDestinationRandom(float fixedDistance)
    {
        Vector2 destinationRandomizer = UnityEngine.Random.insideUnitCircle * fixedDistance;
        float x = destinationRandomizer.x;
        float z = destinationRandomizer.y;
        return new Vector3(x, 0, z);
    }

    public void TakeDamage(float damage)
    {
        if (life <= 0)
        {
            return;
        }

        // is not last strike and reposition
        if (life > 20 && needsReposition)
        {
            return;
        }

        string action = life - damage > 0 ? takeDamageAction : isTrappedAction;
        life -= damage;
        animator.SetBool(action, true);
    }
}
