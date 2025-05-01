using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private float horizontal;
   private float speed = 8f;
   private float jump = 12f;
   public float fallMultiplier   = 2.5f;  
    public float lowJumpMultiplier = 2f;
   private bool isFacingRight = true;

   public bool canMove = true;

   private Animator anim;

   public CoinManager cm;
   

   public bool isInvincible = false;

   [SerializeField] public Rigidbody2D body;
   [SerializeField] private Transform groundCheck;
   [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Set default spawn to last saved position, if available
        if (PlayerData.Instance != null && PlayerData.Instance.lastOverworldPos != Vector3.zero)
        {
            transform.position = PlayerData.Instance.lastOverworldPos;
        }

        // Check if we need to override position for Level 3 or 5
        if (PlayerData.Instance != null)
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (currentScene == "Level 3" && PlayerData.Instance.isFirstTimeInLevel3)
            {
                transform.position = new Vector3(0f, 0f, 0f); // Replace with real Level 3 spawn
                PlayerData.Instance.isFirstTimeInLevel3 = false;
            }
            else if (currentScene == "Level 5" && PlayerData.Instance.isFirstTimeInLevel5)
            {
                transform.position = new Vector3(0f, 0f, 0f); // Replace with real Level 5 spawn
                PlayerData.Instance.isFirstTimeInLevel5 = false;
            }
        }
    }


    void Update()
    {
       
        if(DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 15f;
        }

        else
        {
            speed = 8f;
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
         if (!canMove)
    {
        body.velocity = Vector2.zero;
        return;
    }

        // move left/right
        body.velocity = new Vector2(horizontal * speed, body.velocity.y);

        // now tweak your vertical motion for a floaty jump:
        if (body.velocity.y < 0)
        {
            // we’re falling — apply extra gravity to speed it up
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // you let go of jump while rising — give a “low” jump
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
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
