using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float speedMultiplier = 1f;
    public float dashVelocity;
    Vector3 forward;
    Vector3 right;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        animator = GetComponent<Animator>();

        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
}

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || VerticalInput != 0)
        {
            Move();
            if (dashVelocity > 1)
            {
                animator.SetBool("Dash", true);
            }
            else
                animator.SetBool("Dash", false);
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void Move()
    {

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        Vector3 rightMovement = right * moveSpeed * speedMultiplier * dashVelocity * Time.deltaTime * input.x;
        Vector3 upMovement = forward * moveSpeed * speedMultiplier * dashVelocity * Time.deltaTime * input.z;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }
}

