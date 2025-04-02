using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public string spellName;

    private void Start()
    {
        // If the player already has this spell, destroy the pickup object right away.
        if (PlayerData.Instance.acquiredSpells.Contains(spellName))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!PlayerData.Instance.acquiredSpells.Contains(spellName))
            {
                PlayerData.Instance.AddSpell(spellName);
                Debug.Log("Alexander got a new spell: " + spellName);
            }
           
            Destroy(gameObject);
        }
    }
}
