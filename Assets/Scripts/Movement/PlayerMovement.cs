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

   private Animator anim;

   public CoinManager cm;
   //public PlayerInventory playerInventory;

   public bool isInvincible = false;

   [SerializeField] public Rigidbody2D body;
   [SerializeField] private Transform groundCheck;
   [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();

        //playerInventory = GetComponent<PlayerInventory>();
        if (PlayerData.Instance != null && PlayerData.Instance.lastOverworldPos != Vector3.zero)
        {
            transform.position = PlayerData.Instance.lastOverworldPos;
        }

        
    }

    void Update()
    {
       
        if(DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        
        // Set isMoving only when horizontal movement is detected
    anim.SetBool("isMoving", Mathf.Abs(horizontal) > 0.01f);

    // Jumping logic
    if (Input.GetButtonDown("Jump") && isGrounded())
    {
        body.velocity = new Vector2(body.velocity.x, jump);
        anim.SetBool("isGrounded", false);
        anim.SetBool("isJumping", true);
        anim.SetBool("isFalling", false); 
    }

    // Falling logic
    if (!isGrounded() && body.velocity.y < 0)
    {
         anim.SetBool("isGrounded", false);
        anim.SetBool("isFalling", true);
        anim.SetBool("isJumping", false);
    }

    if(isGrounded())
    {
        anim.SetBool("isFalling", false);
        anim.SetBool("isGrounded", true);
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

    public bool isGrounded()
    {
        anim.SetBool("isGrounded", true);
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
/*
            if(playerInventory != null)
            {
                playerInventory.goldAmount += 1;
            }
*/
        }
    }

    public void StartInvincibility()
    {
        StartCoroutine(InvincibilityFrames());
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(2f); 
        isInvincible = false;
    }

}
