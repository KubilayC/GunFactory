using System;

using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Animator animator;
    private bool isMoving;
    public Transform orientation;
    private RigBuilder rigBuilder;
    float horizontalInput;
    float verticalInput;
    public GameObject joyStick;
    public Joystick joystick;
    public GrabGun grabGun;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rigBuilder = rb.GetComponent<RigBuilder>();
        rb.freezeRotation = true;
        isMoving = false;
        rigBuilder.enabled = false;
    }


    private void FixedUpdate()
    {
        {
            Vector3 movement = new Vector3(joyStick.transform.localPosition.x / 128, 0, joyStick.transform.localPosition.y / 128);

            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            rb.velocity = moveDirection * moveSpeed;

            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, 1000 * Time.fixedDeltaTime);
            }
            if (movement == Vector3.zero && isMoving && !grabGun.bodyToHand.activeSelf && !grabGun.scopeToHand.activeSelf && !grabGun.stockToHand.activeSelf && !grabGun.magToHand.activeSelf)
            {
                animator.SetTrigger("Idle");
                isMoving = false;
            }
            else if (movement != Vector3.zero && !isMoving && !grabGun.bodyToHand.activeSelf && !grabGun.scopeToHand.activeSelf && !grabGun.stockToHand.activeSelf && !grabGun.magToHand.activeSelf)
            {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Run");
                isMoving = true;
            }
            else if (movement == Vector3.zero && isMoving && grabGun.bodyToHand.activeSelf && grabGun.scopeToHand.activeSelf && grabGun.stockToHand.activeSelf && grabGun.magToHand.activeSelf)
            {
                rigBuilder.enabled = true;
                animator.SetTrigger("GunIdle");
                isMoving = false;
            }
            else if(movement != Vector3.zero && !isMoving && grabGun.bodyToHand.activeSelf && grabGun.scopeToHand.activeSelf && grabGun.stockToHand.activeSelf && grabGun.magToHand.activeSelf )
            {
                rigBuilder.enabled = true;
                animator.ResetTrigger("GunIdle");
                animator.SetTrigger("GunRun");
                isMoving = true;
            }
            
        }
    }

}
