using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public List<string> acquiredSpells = new List<string>();
    public List<string> acquiredItems = new List<string>();


    public Vector3 lastOverworldPos;
    public bool isFirstTimeInLevel3 = true;
    public bool isFirstTimeInLevel5  = true;

    // IDs of enemies the player has already defeated:
    public List<string> defeatedEnemies = new List<string>();

    // (Optionally) the ID of the enemy you’re about to fight:
    public string currentBattleEnemyID;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddSpell(string spellName)
    {
        if(!acquiredSpells.Contains(spellName))
        {
            acquiredSpells.Add(spellName);
            Debug.Log("Acquired New Spell: " + spellName);
        }
    }
}
