using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

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
            Vector2 newPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;
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
