using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Key.isCollected)
            {
                Debug.Log("🚪 Kapı açıldı!");
                // İstersen burada animasyon, sahne geçişi, vs. yap
            }
            else
            {
                Debug.Log("🔒 Kapı kilitli. Anahtar lazım.");
            }
        }
    }
}
