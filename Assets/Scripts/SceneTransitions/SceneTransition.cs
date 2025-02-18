using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class SceneTransition : MonoBehaviour
{
    public GameObject button; 

    private void Start()
    {
        
        if (button != null)
        {
            button.SetActive(false);
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (button != null)
            {
                button.SetActive(true); 
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
