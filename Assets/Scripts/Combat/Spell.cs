using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell System/ Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public int damage;
    public int mpCost;
    public Element spellElement;

    
}
