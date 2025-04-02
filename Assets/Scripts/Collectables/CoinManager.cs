using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{

    public int coinCount;
    public TextMeshProUGUI coinText;
    public GameObject gate;


    // Update is called once per frame
    void Update()
    {
        //if(playerInventory != null)
        {
            coinText.text = ": " + coinCount.ToString();

            if(coinCount == 5)
            {
                Destroy(gate);
            }
        }
        
    }
}
