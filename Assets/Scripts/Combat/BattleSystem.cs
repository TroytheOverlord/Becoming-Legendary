using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleStates{ START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    // Player and Enemy Setup Variables
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // Player and Enemy Platform
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    // Player and Enemy Reference to Unit Class Variables
    Unit playerUnit;
    Unit enemyUnit;

    // Battle Dialogue Variable
    public TMPro.TextMeshProUGUI dialogueText;

    // Player and Enemy Stats (Health, MP, Limit, etc)
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleStates states;

    // Spell Variables 
    public GameObject spellMenuPanel;
    public GameObject spellButtonPrefab;
    public Transform spellButtonContainer;
    private List<GameObject> spellButtons = new List<GameObject>();

    // Item Variables 
    public GameObject itemMenuPanel;
    public GameObject itemButtonPrefab;
    public Transform itemButtonContainer;
    private List<GameObject> itemButtons = new List<GameObject>();
    public Inventory playerInventory;

    // Extra Weakness/Crictal Hit Turn Variables
    private bool extraTurnGranted = false;

    // Database of Spells
    public SpellDatabase spellDatabase;

    // Defence Variable 
    private bool isDefending = false;

    // Saves Player Position once enter Combat
    private Vector3 lastPlayerPosition;

    // Scene Variables
    public Scene currentScene;
    private string sceneName;
    private Animator transitionAnim;


    // Character Animator
    private Animator anim;
    private Animator animEnemy;


    // Start is called before the first frame update
    void Start()
    {

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (playerInventory == null)
        {
            Debug.LogError("playerInventory is NULL! Make sure it is assigned.");
        }

        states = BattleStates.START;
        anim = GameObject.Find("BattlePlayer").GetComponent<Animator>();
        animEnemy = GameObject.Find("BattleEnemy").GetComponent<Animator>();

         // Store player position before battle starts
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && sceneName != "Level 1")
        {
            lastPlayerPosition = player.transform.position;
        }
       
        StartCoroutine(SetupBattle());

        spellMenuPanel.SetActive(false);
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

        LoadSpells();
        LoadItems();

        states = BattleStates.PLAYERTURN;
        extraTurnGranted = false; 
        PlayerTurn();
    }

    // Player Physical Attacks
    IEnumerator PlayerAttack()
    {
        animEnemy.SetBool("enemyAttack", false);
        anim.SetBool("isDamaged", false);
        bool extraTurn;
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage, Element.Physical, out extraTurn, isDefending);

        dialogueText.text = playerUnit.unitName + " attacks!";
        anim.SetBool("isAttacking", true);
        animEnemy.SetBool("enemyHurt", true);
        

        enemyHUD.UpdateHP(enemyUnit.currentHp, enemyUnit.maxHp);
        yield return new WaitForSeconds(1f);

        if (isDead || enemyUnit.currentHp <= 0)
        {
            anim.SetBool("isAttacking", false);
            animEnemy.SetBool("enemyHurt", false);
            animEnemy.SetBool("enemyDeath", true);
            states = BattleStates.WON;
            EndBattle();
            yield break;
        }

        if (extraTurn && !extraTurnGranted)
        {
            extraTurnGranted = true;
            dialogueText.text = "CRITICAL HIT! Second Chance!";
            yield return new WaitForSeconds(1f);
            PlayerTurn();
        }
        else
        {
            animEnemy.SetBool("enemyHurt", false);
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        anim.SetBool("isAttacking", false);
        bool extraTurn;
        if(playerUnit.currentHp <= 0) yield break;
       

        dialogueText.text = enemyUnit.unitName + " attacks!";
        animEnemy.SetBool("enemyAttack", true);
        anim.SetBool("isDamaged", true);
        

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage, Element.Physical, out extraTurn, isDefending);

        playerHUD.UpdateHP(playerUnit.currentHp, playerUnit.maxHp);

        isDefending = false;

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            states = BattleStates.LOST;
            EndBattle();
            yield break;
        }

        else
        {
            animEnemy.SetBool("enemyAttack", false);
            anim.SetBool("isDamaged", false);
            states = BattleStates.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(states == BattleStates.WON)
        {
            dialogueText.text = "You won the battle";
            Victory();
        }
        else if(states == BattleStates.LOST)
        {
            dialogueText.text = "You were defeated";
        
            dialogueText.text = "I'm not done yet";
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    //***************************************************Player Magic/ Spell Methods********************************************************//

    // Player Magic Attacks
    IEnumerator PlayerCastSpell(Spell spell)
    {
        if (!playerUnit.CanCastSpell(spell))
        {
            dialogueText.text = "Not enough MP!";
            yield break;
        }

        playerUnit.currentMp -= spell.mpCost;
        playerHUD.UpdateMP(playerUnit.currentMp, playerUnit.maxMp);

        bool extraTurn;
        bool isDead = enemyUnit.TakeDamage(spell.damage, spell.spellElement, out extraTurn, isDefending);

        dialogueText.text = playerUnit.unitName + " uses " + spell.spellName + "!";

        enemyHUD.UpdateHP(enemyUnit.currentHp, enemyUnit.maxHp);
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            states = BattleStates.WON;
            EndBattle();
            yield break;
        }

        if (extraTurn && !extraTurnGranted)  
        {
            extraTurnGranted = true;
            dialogueText.text = "Quick it weak! Second Chance!";
            yield return new WaitForSeconds(1f);
            PlayerTurn();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    // Opens Magic Menu
    public void OpenSpellMenu()
    {
        spellMenuPanel.SetActive(true);
        PopulateSpellMenu();
    }

    // Adds Spells to Magic Menu
    void PopulateSpellMenu()
    {
        // Clears old buttons 
        foreach(GameObject btn in spellButtons)
        {
            Destroy(btn);
        }
        spellButtons.Clear();

        // Creates new buttons upon learning  a new spell
        foreach(Spell spell in playerUnit.knownSpells)
        {
            GameObject buttonObj = Instantiate(spellButtonPrefab, spellButtonContainer);
            Button spellButton = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = spell.spellName + "\n (MP: " + spell.mpCost + ")";
            spellButton.onClick.AddListener(() => CastSpell(spell));

            spellButtons.Add(buttonObj);
        }
    }

    public void CloseSpellMenu()
    {
        spellMenuPanel.SetActive(false);
    }

    void CastSpell(Spell spell)
    {
        StartCoroutine(PlayerCastSpell(spell));
        CloseSpellMenu();
    }

   void LoadSpells()
    {
        if (PlayerData.Instance == null || playerUnit == null || spellDatabase == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        foreach (string spellName in PlayerData.Instance.acquiredSpells)
        {
            Spell spellAsset = spellDatabase.GetSpellByName(spellName);

            if (spellAsset != null)
            {
                Debug.Log("Adding spell: " + spellAsset.spellName);
                playerUnit.knownSpells.Add(spellAsset);
                AddSpellToMenu(spellAsset.spellName);
            }
        }
    }

    void AddSpellToMenu(string spellName)
    {
        GameObject newSpellButton = Instantiate(spellButtonPrefab, spellButtonContainer);
        Button spellButton = newSpellButton.GetComponent<Button>();
        TextMeshProUGUI buttonText = newSpellButton.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = spellName;
        spellButton.onClick.AddListener(() => CastSpellByName(spellName));

        spellButtons.Add(newSpellButton);
    }

    void CastSpellByName(string spellName)
    {
        Spell spellToCast = playerUnit.knownSpells.Find(spell => spell.spellName == spellName);
        if (spellToCast != null)
        {
            StartCoroutine(PlayerCastSpell(spellToCast));
            CloseSpellMenu();
        }
        else
        {
            Debug.LogWarning("Spell not found: " + spellName);
        }
    }

    //**********************************************************Player Defence Methods*************************************************//

    public void OnDefendButton()
    {
        if(states != BattleStates.PLAYERTURN) return;

        isDefending = true;
        dialogueText.text = playerUnit.unitName + " braces themselves";

        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    //**********************************************************Player Flee Methods*************************************************//

    public void OnFleeButton()
    {
        if(states != BattleStates.PLAYERTURN) return;

        StartCoroutine(AttemptFlee());
    }

    IEnumerator AttemptFlee()
    {
        float fleeChance = (float)playerUnit.speed / (float)enemyUnit.speed; 
        fleeChance = Mathf.Clamp(fleeChance * 100, 10f, 90f); 

        int roll = Random.Range(1, 101); 
        if (roll <= fleeChance)
        {
            dialogueText.text = "You managed to escape!";
            StartCoroutine(RoomTransition());
            ReturnToOverworld(true);
        }
        else
        {
            dialogueText.text = "Couldn't escape!";
            yield return new WaitForSeconds(1f);
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void ReturnToOverworld(bool fledSuccessfully)
    {
        StartCoroutine(ReturnToOverworldSequence(fledSuccessfully));
    }

    private IEnumerator ReturnToOverworldSequence(bool fledSuccessfully)
    {
        if (fledSuccessfully)
        {
            dialogueText.text = "Escaping...";
            yield return new WaitForSeconds(2f); // Add a delay before switching scenes

            SceneManager.LoadScene("Level 3");
             

            // Give the player invincibility after fleeing
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().StartInvincibility();
            }
           
        }
    }


    //**********************************************************Player Victory Methods*************************************************//

    private void Victory()
    {
        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        dialogueText.text = "You won the battle!";

        // Mark that ID defeated
        var id = PlayerData.Instance.currentBattleEnemyID;
        if (!string.IsNullOrEmpty(id) && 
            !PlayerData.Instance.defeatedEnemies.Contains(id))
        {
            PlayerData.Instance.defeatedEnemies.Add(id);
        }

        StartCoroutine(RoomTransition());

        // Restore player's position
        yield return new WaitForSeconds(0.5f); // Small buffer to allow scene to load
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastPlayerPosition;
        }
    }


    //**********************************************************Item Methods*************************************************//

    public void UseItem(Unit targetUnit, Item item)
    {
        if (item.itemName == "Health Potion")
        {
            targetUnit.UseHealthPotion(item.hpRestore);
            Debug.Log(targetUnit.unitName + " restored " + item.hpRestore + " HP!");
            CloseItemMenu();
        }
        else if (item.itemName == "Mp Potion")
        {
            targetUnit.UseManaPotion(item.mpRestore);
            playerHUD.UpdateMP(playerUnit.currentMp, playerUnit.maxMp);
            Debug.Log(targetUnit.unitName + " restored " + item.mpRestore + " MP!");
            CloseItemMenu();
        }
        
        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    public void UpdateItemMenuUI()
    {
        // Ensure inventory is valid
        if (playerInventory == null)
        {
            Debug.LogError("Player inventory is null! Make sure Inventory is assigned.");
            return;
        }

        // Clear previous buttons
        foreach (GameObject button in itemButtons)
        {
            Destroy(button);
        }
        itemButtons.Clear();

        // Generate new buttons for each item
        foreach (var kvp in playerInventory.items)
        {
            Item item = kvp.Value; // Extract the actual item object

            GameObject newItemButton = Instantiate(itemButtonPrefab, itemButtonContainer);
            TextMeshProUGUI buttonText = newItemButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"{item.itemName} x{item.itemQunatity}";

            newItemButton.GetComponent<Button>().onClick.AddListener(() => UseItem(playerUnit, item));
            itemButtons.Add(newItemButton); // Store buttons for clearing later
        }
    }

    void PopulateItemMenu()
    {
        Debug.Log("Populating item menu...");

        // Clear existing buttons
        foreach (GameObject btn in itemButtons)
        {
            Destroy(btn);
        }
        itemButtons.Clear();

        Debug.Log($"Number of items in inventory: {playerInventory.items.Count}");

        // Create new buttons for each item
        foreach (var kvp in playerInventory.items)
        {
            Item item = kvp.Value;
            Debug.Log($"Item found: {item.itemName}, Quantity: {item.itemQunatity}");

            GameObject buttonObj = Instantiate(itemButtonPrefab, itemButtonContainer);
            Button itemButton = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = $"{item.itemName} x{item.itemQunatity}";
            itemButton.onClick.AddListener(() => UseItem(playerUnit, item));

            itemButtons.Add(buttonObj);
        }
        Debug.Log("Item menu populated.");
    }


    void LoadItems()
    {
        if (PlayerData.Instance == null || playerInventory == null)
        {
            Debug.LogError("Missing references for loading items!");
            return;
        }

        foreach (string itemName in PlayerData.Instance.acquiredItems)
        {
            Item itemAsset = GetItemByName(itemName); 

            if (itemAsset != null)
            {
                Debug.Log("Adding item: " + itemAsset.itemName);
                Debug.Log($"Loading item into inventory: {itemAsset.itemName}");
                playerInventory.AddItem(itemAsset);
            }

            else
            {
                Debug.LogError($"Item '{itemName}' not found in predefined list!");
            }
        }
    }

    Item GetItemByName(string itemName)
    {
        switch (itemName)
        {
            case "Health Potion":
                return new Item { itemName = "Health Potion", itemQunatity = 1, hpRestore = 50};
            case "Mp Potion":
                return new Item { itemName = "Mp Potion", itemQunatity = 1, mpRestore = 30};
            default:
                return null;
        }
    }

   public void OpenItemMenu()
    {
        Debug.Log("Opening item menu...");
       itemMenuPanel.SetActive(true);
       PopulateItemMenu();
    }

    public void CloseItemMenu()
    {
        itemMenuPanel.SetActive(false);
    }

    //**********************************************************Scene Transition Methods*************************************************//

     IEnumerator RoomTransition()
    {
        if(sceneName == "Level 2")
        {
             yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Level 3");
        }

        else if(sceneName == "Level 4")
        {
             yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("End Scene");
        }
       
    }
}
