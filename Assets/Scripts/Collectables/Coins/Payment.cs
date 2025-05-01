using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payment : MonoBehaviour
{
   public int payment;
   public GameObject payZone;
    public GameObject barrier; 
    private CoinManager coinManager;

    public bool playerInZone = false;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }

    void Update()
        {
            if (playerInZone && Input.GetKeyDown(KeyCode.P))
            {
                if (coinManager.coinCount >= payment)
                {
                    coinManager.coinCount -= payment;
                    Destroy(barrier);
                    playerInZone = false; 
                }
                else
                {
                    Debug.Log("Not enough coins!");
                }
            }
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}

