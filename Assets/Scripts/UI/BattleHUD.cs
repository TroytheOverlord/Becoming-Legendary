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
    public TMPro.TextMeshProUGUI mpText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lv: " + unit.unitLevel;
        UpdateHP(unit.currentHp, unit.maxHp);
        UpdateMP(unit.currentMp,unit.maxMp);
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
        hpText.text = $"Hp: {(int)currentHp}/{(int)maxHp}"; 
    }

    public void UpdateMP(float currentMp, float maxMp)
    {
        mpText.text = $"Mp: {(int)currentMp}/{(int)maxMp}";
    }
}
