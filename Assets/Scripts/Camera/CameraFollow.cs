using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public float zoom = 5f;
    private Vector3 targetPoint = Vector3.zero;
   public PlayerMovement player;
   public float moveSpeed;
   public float lookAheadDistance = 5f, lookAheadSpeed = 3f;
   private float lookOffset;
   private bool isFalling;
   public float maxVertOffset = 5f; 
   private Camera cam;

   void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Called in editor when values are changed in the Inspector.
    void OnValidate()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        if (cam != null)
        {
            cam.orthographicSize = zoom;
        }
    }

   void Start()
   {
    targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
   }

    // Update is called once per frame
    void Update()
    {
        // Adjust the y position by adding yOffset to the player's y position.
        if(player.isGrounded())
        {
            targetPoint.y = player.transform.position.y;
        }

        if(transform.position.y - player.transform.position.y > maxVertOffset)
        {
            isFalling = true;
        }

        if(isFalling)
        {
            targetPoint.y = player.transform.position.y;

            if(player.isGrounded())
            {
                isFalling = false;
            }
        }

        if(player.body.velocity.x > 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }
        else if(player.body.velocity.x < 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }

        targetPoint.x = player.transform.position.x + lookOffset;

        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }
}
