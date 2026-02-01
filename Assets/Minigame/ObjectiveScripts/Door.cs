using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public enum DoorState
    {
        Locked,
        Open
    }

    [Header("Door Settings")]
    public DoorState doorState = DoorState.Locked;

    private void Start()
    {
        Key.isCollected = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        switch (doorState)
        {
            case DoorState.Open:
            AudioManager.Instance.PlaySFX(3);
                Debug.Log("🚪 Kapı zaten açık. Geçebilirsin.");
                SceneLoader.Instance.LoadScene(sceneName, GameState.Playing);
                break;

            case DoorState.Locked:
                if (Key.isCollected)
                {
                    SceneLoader.Instance.LoadScene(sceneName, GameState.Playing);
                    // Burada istersen kapıyı açabilirsin
                    doorState = DoorState.Open;
                    // İsteğe bağlı animasyon, ses, sahne geçişi vs.
                }
                else
                {
                    Debug.Log("🔒 Kapı kilitli. Anahtar lazım.");
                }
                break;
        }
    }
}
