using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int damage;
    
    public int maxHp = 100;
    public int currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public bool TakeDamage(int dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); 

        return currentHp <= 0;
    }
}
