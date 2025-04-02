using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TMP_FontAsset customFont;

    private Story currentStory;

    public bool dialogueIsPlaying {get; private set; }

    private bool openShopAfterDialogue = false;

   private static DialogueManager instance;

   private void Awake()
   {
        if(instance != null)
        {
            Debug.LogWarning("More than one dialogue manager found in scene");
        }

        instance = this;

   }

   public static DialogueManager GetInstance()
   {
        return instance;
   }

   private void Start()
   {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        if (customFont != null)
        {
            dialogueText.font = customFont;
        }
   }

   private void Update()
   {
        // Returns if dialogue is not playing 
        if(!dialogueIsPlaying)
        {
            return;
        }

        // Continues to the next line in the dialogue when button is pressed
        if(Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }
   }

   public void EnterDialogueMode(TextAsset inkJSON, bool isShopkeeper)
   {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Stops Player Movemnet During Dialogue
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = false;

        openShopAfterDialogue = isShopkeeper;

        ContinueStory();
   }

   private void ExitDialogueMode()
   {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // Reenables Player Movemnet During Dialogue
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = true;
   }

   private void ContinueStory()
   {
         if(currentStory.canContinue)
        {
            // Sets text fo the current dialogue line 
            dialogueText.text = currentStory.Continue();
        }

        else
        {
            ExitDialogueMode();
        }
   }
   
}
