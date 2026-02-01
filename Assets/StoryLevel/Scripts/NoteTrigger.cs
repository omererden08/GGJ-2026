using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [Header("Note Settings")]
    [SerializeField] private GameObject noteObject;
    [SerializeField] private int noteSoundIndex = 4;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    

    private bool playerInRange = false;
    private Player player;

    private bool isNoteActive = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey) && !isNoteActive)
        {
            if (player != null && player.IsCarrying())
                return;
            ShowNote();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Holder tag'i ile kontrol et
        if (other.CompareTag("Holder"))
        {
            playerInRange = true;
            
            // Player component'ini parent'tan al
            if (player == null)
            {
                player = other.GetComponentInParent<Player>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Holder"))
        {
            playerInRange = false;
        }
    }

    private void ShowNote()
    {
        if (noteObject != null && !isNoteActive)
        {
            StartCoroutine(ShowNoteCoroutine());
        }
    }

    private System.Collections.IEnumerator ShowNoteCoroutine()
    {
        isNoteActive = true;
        // Oyuncuyu devre dışı bırak
        if (player != null)
            player.enabled = false;

        // Ses çal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(noteSoundIndex);
        }

        // Notu göster
        noteObject.SetActive(true);

        // Bekle
        yield return new WaitForSeconds(displayDuration);

        // Notu gizle
        noteObject.SetActive(false);

        // Oyuncuyu tekrar aktif et
        if (player != null)
            player.enabled = true;

        // Player devre dışı olduğu için trigger exit çalışmayabilir, manuel sıfırla
        playerInRange = false;
        isNoteActive = false;
    }
}