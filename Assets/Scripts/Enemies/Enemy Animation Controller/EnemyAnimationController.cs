using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private Enemy enemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        // Sample code to set animation parameters
        // For example: animator.SetBool("IsIdle", (enemy's current state is IdleState));
    }


    public void SetWalking(bool value)
    {
        animator.SetBool("IsWalking", value);
    }



}

