using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopDamage : MonoBehaviour
{
    private Animator animator;
    private readonly string takeDamageAction = "takeDamage";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StopDamage()
    {
        animator.SetBool(takeDamageAction, false);
    }
}
