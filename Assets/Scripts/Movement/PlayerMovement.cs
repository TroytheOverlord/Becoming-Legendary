using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private float horizontal;
   private float speed = 8f;
   private float jump = 12f;
   private bool isFacingRight = true;

   public bool canMove = true;

   public CoinManager cm;

   [SerializeField] private Rigidbody2D body;
   [SerializeField] private Transform groundCheck;
   [SerializeField] private LayerMask groundLayer;

    void Update()
    {
       
        if(DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jump);
        }

        if(Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
         if(!canMove)
        {
            body.velocity = Vector2.zero;
            return;
        }

        body.velocity = new Vector2(horizontal * speed, body.velocity.y);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coins"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }
}
