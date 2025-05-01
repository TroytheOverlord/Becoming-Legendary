using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject gamplayMenu;

    public void OpenGameMenu()
    {
        gamplayMenu.SetActive(true);
    }

    public void CloseGameMenu()
    {
        gamplayMenu.SetActive(false);
    }
}
