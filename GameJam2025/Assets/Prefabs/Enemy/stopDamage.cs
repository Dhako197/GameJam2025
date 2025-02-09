using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopDamage : MonoBehaviour
{
    private Animator animator;
    private readonly string takeDamageAction = "takeDamage";
    private readonly string repositionAction = "reposition";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // damage animation has exit time
    // next state must call this one
    public void StopDamage()
    {
        animator.SetBool(takeDamageAction, false);
        animator.SetBool(repositionAction, true);
    }
}
