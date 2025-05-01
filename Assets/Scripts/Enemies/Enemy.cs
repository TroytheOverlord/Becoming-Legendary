using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Unique ID for this enemy. Can be a GUID or just a unique string per prefab instance.")]
   public string enemyID;


    void Awake()
    {
        if (PlayerData.Instance != null 
            && PlayerData.Instance.defeatedEnemies.Contains(enemyID))
        {
            // we already beat this one!
            Destroy(gameObject);
        }
    }
}
