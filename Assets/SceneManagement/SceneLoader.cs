using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1); // Start fully black
            fadeImage.DOFade(0f, fadeDuration);      // Fade in at start
        }
        else
        {
            Debug.LogWarning("Fade image not assigned in SceneLoader.");
        }
    }

    public void LoadScene(string sceneName, GameState newStateAfterLoad)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, newStateAfterLoad));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, GameState newState)
    {
        yield return FadeToBlack();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        GameManager.Instance.ChangeState(newState);
        yield return FadeFromBlack();
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeImage == null)
            yield break;

        fadeImage.raycastTarget = true;
        yield return fadeImage.DOFade(1f, fadeDuration).WaitForCompletion();
    }

    private IEnumerator FadeFromBlack()
    {
        if (fadeImage == null)
            yield break;

        yield return fadeImage.DOFade(0f, fadeDuration).WaitForCompletion();
        fadeImage.raycastTarget = false;
    }
}
