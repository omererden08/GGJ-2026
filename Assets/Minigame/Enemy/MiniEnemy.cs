using UnityEngine;
using System.Collections;
public class MiniEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.PlayerLocked(); // GameManager'daki GameOver tetiklenir
        }
    }


}
