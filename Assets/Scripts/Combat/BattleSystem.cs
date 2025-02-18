using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BattleStates{ START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TMPro.TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;


    public BattleStates states;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleStates.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Engaging combat with " + enemyUnit.unitName;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        states = BattleStates.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        //Damages the enemy
        enemyUnit.TakeDamage(playerUnit.damage);

        dialogueText.text = playerUnit.unitName + " attacks!";

        enemyHUD.UpdateHP(enemyUnit.currentHp, enemyUnit.maxHp);

        yield return new WaitForSeconds(2f);

        //Check if the enemy is dead 
        if(isDead || enemyUnit.currentHp <= 0)
        {
            states = BattleStates.WON;
            EndBattle();
            yield break;
        }

        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        // Change state based on what happened
    }

    IEnumerator EnemyTurn()
    {
        if(playerUnit.currentHp <= 0) yield break;
       

        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.UpdateHP(playerUnit.currentHp, playerUnit.maxHp);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            states = BattleStates.LOST;
            EndBattle();
            yield break;
        }

        else
        {
            states = BattleStates.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(states == BattleStates.WON)
        {
            dialogueText.text = "You won the battle";
        }
        else if(states == BattleStates.LOST)
        {
            dialogueText.text = "You were defeated";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action: ";
    }

    public void OnAttackButtonButton()
    {
        if(states != BattleStates.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
    }

    
}
