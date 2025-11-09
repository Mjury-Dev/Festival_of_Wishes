using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runMultiplier = 2f; // how much faster when running (Shift)

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;
    private Vector2 lastMoveDir = Vector2.down; // Default facing down
    [HideInInspector] public bool isAttacking; // referenced by PlayerCombat

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // --- Skip movement input if attacking ---
        if (isAttacking) return;

        // --- MOVEMENT INPUT ---
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement
        if (input.x != 0) input.y = 0;

        // --- STORE LAST NON-ZERO DIRECTION ---
        if (input != Vector2.zero)
        {
            lastMoveDir = input;
        }

        // --- ANIMATION UPDATES ---
        // Always face the last direction, even when idle
        animator.SetFloat("moveX", lastMoveDir.x);
        animator.SetFloat("moveY", lastMoveDir.y);
        animator.SetBool("isMoving", input != Vector2.zero);
    }

    private void FixedUpdate()
    {
        if (!isAttacking && input != Vector2.zero)
        {
            // Check if player is holding Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            float currentSpeed = moveSpeed;
            if (isRunning)
            {
                currentSpeed *= runMultiplier;
            }

            Vector2 newPos = rb.position + input * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // --- expose last direction for combat script ---
    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDir;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
