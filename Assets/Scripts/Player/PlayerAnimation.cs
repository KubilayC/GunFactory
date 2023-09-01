using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    private bool isMoving;
    public bool isWalking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
    }

    private void Update()
    {
        isMoving = playerController.IsMoving();
        if (isMoving && !isWalking)
        {
            isWalking = true;
            //animator.SetBool("IsRunning", true);
        }
        else
        {
            isWalking = false;
            //animator.SetBool("IsRunning", false);
        }
        //animator.SetBool("IsBreathing", !isMoving);
    }
}
