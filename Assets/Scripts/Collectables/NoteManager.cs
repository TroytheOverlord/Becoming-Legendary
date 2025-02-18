using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteManager : MonoBehaviour
{
    // Singleton Instance
    public static NoteManager instance;

    // The UI Panel for viewing collected notes
    public GameObject journalUI;
    // Text UI that displays notes 
    public TextMeshProUGUI noteTextDisplay;
    // Prefab for note buttons in the journal
    public GameObject noteEntryPrefab;
    // Parent object to hold note buttons
    public Transform noteListParent;

    // Storage for the collect notes 
    private List<string> collectedNotes = new List<string>();
    // Stores buttons to acces notes
    private List<GameObject> noteButtons = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else{
            Destroy(gameObject);
        }
    }

    public void AddNote(string noteText)
    {
        collectedNotes.Add(noteText);
        CreateNoteEntry(noteText);
    }

    // Adds notes to journal upon collecting them
    private void CreateNoteEntry(string noteText)
    {
        GameObject newNoteSlot = Instantiate(noteEntryPrefab, noteListParent);
        newNoteSlot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        newNoteSlot.GetComponent<Button>().onClick.AddListener(() => ShowNote(noteText));
        noteButtons.Add(newNoteSlot);
    }

    // Displays the selected note in the journal
    public void ShowNote(string noteText)
    {
        noteTextDisplay.text = noteText;
    }

    // Toggles the Journal on and off
    public void ToggleJournal()
    {
        journalUI.SetActive(!journalUI.activeSelf);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }
}
