using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public List<string> acquiredSpells = new List<string>();
    public List<string> acquiredItems = new List<string>();


    public Vector3 lastOverworldPos;

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
