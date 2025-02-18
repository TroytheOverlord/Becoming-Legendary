using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI hpText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lv: " + unit.unitLevel;
        UpdateHP(unit.currentHp, unit.maxHp);
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
        hpText.text = $"{(int)currentHp}/{(int)maxHp}"; // Displays as "currentHP/maxHP"
    }
}
