using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }
    public Animator transitionAnim;
    public string sceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TransitionToScene(string name)
    {
        name = sceneName;
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
        }

        else
        {
            Debug.LogWarning("Transition Animator not assigned!");
        }

        SceneManager.LoadScene(sceneName);
    }
}
