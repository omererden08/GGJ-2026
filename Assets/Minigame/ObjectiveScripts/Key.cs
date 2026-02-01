using UnityEngine;

public class Key : MonoBehaviour
{
    public static bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            AudioManager.Instance.PlaySFX(2);
            Debug.Log("🔑 Anahtar alındı!");
            Destroy(gameObject); // Anahtarı yok et
        }
    }
}
