using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 3f;
    private bool isFacingRight = true;
    private float direction = 1f; 

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    void FixedUpdate()
    {
        // Move the enemy in the current direction
        body.velocity = new Vector2(direction * speed, body.velocity.y);

        // Check for ground and walls to determine if the enemy should turn around
        if (isTouchingWall() || isGroundAhead())
        {
            Flip();
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool isGroundAhead()
    {
        // Check if there is ground ahead by placing the groundCheck object slightly in front of the enemy
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        direction *= -1f;

        // Flip the enemy's sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
