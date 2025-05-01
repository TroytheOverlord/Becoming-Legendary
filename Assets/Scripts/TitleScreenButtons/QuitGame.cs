using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Exited Game");
        Application.Quit();
    }
}
