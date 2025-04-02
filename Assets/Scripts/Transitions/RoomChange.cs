using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomChange : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;
   //public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (other.CompareTag("Player") && !player.isInvincible)
        {
            // Store last overworld position
            PlayerData.Instance.lastOverworldPos = other.transform.position;

            // Load battle scene
            StartCoroutine(RoomTransition());
        }
    }

    IEnumerator RoomTransition()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }


}
