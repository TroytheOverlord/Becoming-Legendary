using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldTransition : MonoBehaviour
{
    public string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneController.instance.LoadScene(levelName);
        }
    }
}

