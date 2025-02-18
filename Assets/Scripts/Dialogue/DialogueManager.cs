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

    /*
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    */

    private Story currentStory;

    public bool dialogueIsPlaying {get; private set; }

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

        /*
        // Gets the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        */
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

   public void EnterDialogueMode(TextAsset inkJSON)
   {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Stops Player Movemnet During Dialogue
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = false;

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

            // Display choices, if any for this dialogue line 
            //DisplayChoices();
        }

        else
        {
            ExitDialogueMode();
        }
   }

   /*private void DisplayChoices()
   {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices: " + currentChoices.Count);
        }

        int index = 0;

        //Enables and Initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // Goes through teh remaining choices the UI supports and make sure they're hidden
        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
   }
   */
   
}
