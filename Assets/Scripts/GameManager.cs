using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public enum GameState
{
    CutScene,
    Playing,
    Paused,
    GameOver,
    MainMenu
}

public class GameManager : MonoBehaviour
{
    [Header("Menu Prefabs")]
    [SerializeField] private GameObject creditsCanvasPrefab;

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
        ChangeState(GameState.MainMenu);
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
            case GameState.MainMenu:
                HandleMainMenu();
                break;
        }
    }

    private void HandleCutScene()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: CutScene");
        // Cutscene oynat�l�r (kontroller devre d���, UI kapal� olabilir)
        // �ste�e ba�l� olarak oyuncu giri�i engellenebilir
    }
    public void StartGameButton()
    {
        StartCoroutine(StartGame());
    }
    public void QuitGameButton()
    {
        StartCoroutine(QuitMenu());
    }
    public void CreditsGameButton()
    {
        StartCoroutine(Credits());
    }
    private IEnumerator StartGame()
    {
        AudioManager.Instance.PlaySFX(1);
        yield return new WaitForSecondsRealtime(1f);
        SceneLoader.Instance.LoadScene("Gameplay", GameState.Playing);
    }
    private void HandleMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: MainMenu");
        AudioManager.Instance.PlayMusic(0);
        AudioManager.Instance.SetMusicVolume(1f);
        AudioManager.Instance.SetSFXVolume(1f);
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f;
        Debug.Log("Game State: Playing");
        AudioManager.Instance.SetMusicVolume(0.5f);
        // Gameplay ba�lar
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Paused");
        // Pause men�s� g�sterilir
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game State: Game Over");
        
        StartCoroutine(RestartLevelDelayed());

        // UI a��labilir
        // Fade, ses, animasyon tetiklenebilir
        // Restart veya Men� tu�u aktif edilebilir
    }


    private void Update()
    {
        // Escape ile pause sadece Playing veya Paused durumundayken ge�erli
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
            // Sahne yeniden y�klendi�inde otomatik Playing'e ge�
            ChangeState(GameState.Playing);
        }
    }
    private IEnumerator QuitMenu()
    {
        AudioManager.Instance.PlaySFX(1); 
        yield return new WaitForSecondsRealtime(1f);
        Application.Quit();
    }
    private IEnumerator Credits()
    {
        AudioManager.Instance.PlaySFX(1);
        yield return new WaitForSecondsRealtime(1f);
        Instantiate(creditsCanvasPrefab);
    }
}
