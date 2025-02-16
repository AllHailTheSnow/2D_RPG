using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    // The speed at which the enemy moves
    [SerializeField] private float moveSpeed = 2f;

    // The rigidbody of the enemy
    private Rigidbody2D rb;
    // The direction the enemy is moving in
    private Vector2 moveDir;

    private Knockback knockback;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private void Awake()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // Get the knockback component
        knockback = GetComponent<Knockback>();
        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get the animator component
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // If the enemy is currently being knocked back, return
        if (knockback.GetingKnockedback) { return; }

        // Move the enemy in the direction of moveDir
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        if(moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if(moveDir != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        // Calculate the direction to the target position
        moveDir = targetPosition;
    }
}
