using UnityEngine;

public class MatchPickup : MonoBehaviour
{
    private bool playerInRange = false;
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySFX(2);
            GameManager.Instance.HasMatch = true;
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Holder"))
        {
            playerInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Holder"))
        {
            playerInRange = false;
        }
    }
}