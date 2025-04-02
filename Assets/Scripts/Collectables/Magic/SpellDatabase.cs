using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpellDatabase", menuName = "Spell System/Spell Database")]
public class SpellDatabase : ScriptableObject
{
    public List<Spell> allSpells = new List<Spell>();
    
    private Dictionary<string, Spell> spellLookup;

    public void Initialize()
    {
        spellLookup = new Dictionary<string, Spell>();
        foreach (Spell spell in allSpells)
        {
            spellLookup[spell.spellName] = spell;
        }
        
    }

    public Spell GetSpellByName(string spellName)
    {
        if (spellLookup == null) Initialize();

        if (spellLookup.TryGetValue(spellName, out Spell spell))
            return spell;

        Debug.LogError("Spell not found in database: " + spellName);
        return null;
    }
}

