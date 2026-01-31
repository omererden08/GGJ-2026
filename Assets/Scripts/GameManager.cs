using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public enum GameState
{
    CutScene,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += OnPlayerDied;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= OnPlayerDied;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnPlayerDied()
    {
        ChangeState(GameState.GameOver);
    }

    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.CutScene:
                HandleCutScene();
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

    private void HandleCutScene()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: CutScene");
        // Cutscene oynatýlýr (kontroller devre dýþý, UI kapalý olabilir)
        // Ýsteðe baðlý olarak oyuncu giriþi engellenebilir
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: Playing");
        // Gameplay baþlar
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Paused");
        // Pause menüsü gösterilir
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Game Over");
        
        StartCoroutine(RestartLevelDelayed());

        // UI açýlabilir
        // Fade, ses, animasyon tetiklenebilir
        // Restart veya Menü tuþu aktif edilebilir
    }


    private void Update()
    {
        // Escape ile pause sadece Playing veya Paused durumundayken geçerli
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
                ChangeState(GameState.Paused);
            else if (CurrentState == GameState.Paused)
                ChangeState(GameState.Playing);
        }
    }

    private IEnumerator RestartLevelDelayed()
    {
        yield return new WaitForSecondsRealtime(2f); // 2 saniye bekle (zaman donsa bile)
        RestartLevel();
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CurrentState == GameState.GameOver)
        {
            // Sahne yeniden yüklendiðinde otomatik Playing'e geç
            ChangeState(GameState.Playing);
        }
    }

}
