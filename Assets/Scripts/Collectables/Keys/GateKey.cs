using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKey : MonoBehaviour
{
    public GameObject gate;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.CompareTag("Player"))
        {
            gate.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
