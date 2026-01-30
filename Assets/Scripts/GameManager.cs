using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    // Current Game State
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne geçiþlerinde kaybolmasýn
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu); // Oyun baþladýðýnda ana menüde baþlasýn
    }

    // GameState deðiþtirici
    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;

        // Her durum için gerekli aksiyonlarý burada tanýmla
        switch (CurrentState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    private void HandleMainMenu()
    {
        Time.timeScale = 1f; // Menüde zaman normal akabilir
        Debug.Log("Game State: Main Menu");
        // Ana menü UI’si açýlabilir
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: Playing");
        // Oyunu baþlat
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Paused");
        // Pause UI gösterilebilir
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Game Over");
        // Game over ekraný gösterilebilir
    }

    // Geliþtirici kolaylýðý için bazý kýsayollar (isteðe baðlý)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
                ChangeState(GameState.Paused);
            else if (CurrentState == GameState.Paused)
                ChangeState(GameState.Playing);
        }
    }
}
