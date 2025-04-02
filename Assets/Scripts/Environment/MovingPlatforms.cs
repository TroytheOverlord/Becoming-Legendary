using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private Vector3 nextPosition;
    [SerializeField] private float tolerance = 0.1f; 

    void Start()
    {
        // Force target positions to have z = 0
        Vector3 posB = new Vector3(pointB.position.x, pointB.position.y, 0);
        Vector3 posA = new Vector3(pointA.position.x, pointA.position.y, 0);
        
        nextPosition = posB;
    
        // Set sorting order if needed
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 5; // Adjust as needed
        }
    }

    void Update()
    {
        // Force the platform's z position to 0 each frame
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); 
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        // Check if close enough to target position
        if (Vector3.Distance(transform.position, nextPosition) < tolerance)
        {
            // Force target positions to z=0 for comparison and assignment
            Vector3 posA = new Vector3(pointA.position.x, pointA.position.y, 0);
            Vector3 posB = new Vector3(pointB.position.x, pointB.position.y, 0);

            // Swap target position using a distance check
            if (Vector3.Distance(nextPosition, posA) < tolerance)
            {
                nextPosition = posB;
            }
            else
            {
                nextPosition = posA;
            }
        }

        // Ensure SpriteRenderer is enabled
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && !sr.enabled)
        {
            Debug.Log("Platform turned invisible! Enabling SpriteRenderer.");
            sr.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(ResetParent), 0.1f);
        }
    }

    private void ResetParent()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.SetParent(null);
        }
    }
}
