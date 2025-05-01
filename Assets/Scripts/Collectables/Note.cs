using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{

    [TextArea(3,10)]
    public string noteText;
    public GameObject noteUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Makes sure only the player can collect lore notes
        if(other.CompareTag("Player"))
        {
            // Adds current note to journal 
            NoteManager.instance.AddNote(noteText);
            // Displays Notes
            StartCoroutine(DisplayNote());
        }
    }

   IEnumerator DisplayNote()
   {
        //Shows the UI
        noteUI.SetActive(true);
        // Sets the text
        noteUI.GetComponentInChildren<TextMeshProUGUI>().text = noteText;

        //Shows text for 5 seconds
        yield return new WaitForSeconds(3f);

        // Hides the UI again
        noteUI.SetActive(false);
        // Removes the note from the screen 
        Destroy(gameObject);
   }
}
