using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public GameObject door;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            door.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}

