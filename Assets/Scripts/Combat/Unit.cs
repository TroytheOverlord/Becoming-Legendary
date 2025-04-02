using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Elemental Damage Types
public enum Element {Physical, Fire, Ice, Wind, Light}

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHp = 100;
    public int currentHp;
    public int maxMp = 50;
    public int currentMp;

    public int speed = 15;
    public Element unitElement = Element.Physical;

    public List<Spell> knownSpells = new List<Spell>();

    private Dictionary<Element, float> weaknesses = new Dictionary<Element, float>();
    private Dictionary<Element, float> resistance = new Dictionary<Element, float>();

    // Spell Animations
    public GameObject fireballPrefab;
    public Transform spawnPoint;
    public float fireballSpeed = 5f;

    public GameObject caster;


    private void Start()
    {
        currentHp = maxHp;
        currentMp = maxMp;

        SetWeakness(Element.Fire, 2f);
        SetResistance(Element.Ice, 2f);


    }

    public void SetWeakness(Element element, float multiplier)
    {
        weaknesses[element] = multiplier;
    }

    public void SetResistance(Element element, float multiplier)
    {
        resistance[element] = multiplier;
    }

    public bool TakeDamage(int dmg, Element attackElement, out bool extraTurn, bool isDefending)
    {
        extraTurn = false;
        float damageMultiplier = 1f;

        if(isDefending)
        {
            dmg /= 2;
        }

        if(!isDefending)
        {
            //Checks for weaknesses
            if(weaknesses.ContainsKey(attackElement))
            {
                damageMultiplier = weaknesses[attackElement];
                extraTurn = true;
            }

            else if (resistance.ContainsKey(attackElement))
            {
                damageMultiplier = resistance[attackElement];
            }
        }

        int finalDamage = Mathf.RoundToInt(dmg * damageMultiplier);
        currentHp -= finalDamage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); 

        isDefending = false;

        return currentHp <= 0;
    }

    // Player Learns New Spell upon receiving a spell scroll
    public void LearnSpell(Spell spell)
    {
        if(!knownSpells.Contains(spell))
        {
            knownSpells.Add(spell);
        }
    }


    // Allows Player to cast spell
    public bool CanCastSpell(Spell spell)
    {
        return currentMp >= spell.mpCost;
    }

    public void UseHealthPotion(int healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }

    // Restores MP up to maxMp
    public void UseManaPotion(int manaAmount)
    {
        currentMp += manaAmount;
    
        // Ensure MP does not exceed max MP
        if (currentMp > maxMp)
        {
            currentMp = maxMp;
        }

        Debug.Log(unitName + " now has " + currentMp + " MP.");
    }
}
