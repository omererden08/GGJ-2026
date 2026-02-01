using UnityEngine;
using System;
using UnityEngine.Playables; // Timeline desteği için

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Options")]
    [SerializeField] private bool useAutoEndTime = true;
    [SerializeField] private float autoEndTime = 5f;

    [SerializeField] private bool allowSkipWithClick = true;

    [Header("Timeline Support")]
    [SerializeField] private bool useTimeline = false;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private string sceneName;
    private bool cutsceneActive = false;
    private float timer = 0f;

    public static event Action OnCutsceneEnd;

    private void Start()
    {
        StartCutscene();
    }

    private void Update()
    {
        if (!cutsceneActive) return;

        // Oyuncu mouse sol tıklaması ile geçmek isterse
        if (allowSkipWithClick && Input.GetMouseButtonDown(0))
        {
            EndCutscene();
            return;
        }

        // Otomatik bitiş süresi kullanılıyorsa
        if (useAutoEndTime && !useTimeline)
        {
            timer += Time.deltaTime;
            if (timer >= autoEndTime)
            {
                EndCutscene();
            }
        }
    }

    private void StartCutscene()
    {
        cutsceneActive = true;
        timer = 0f;

        if (useTimeline && timeline != null)
        {
            timeline.Play();
            timeline.stopped += OnTimelineFinished;
        }

        GameManager.Instance.ChangeState(GameState.CutScene);
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        EndCutscene();
    }

    private void EndCutscene()
    {
        if (!cutsceneActive) return;

        cutsceneActive = false;

        if (timeline != null)
            timeline.stopped -= OnTimelineFinished;

        Debug.Log("🎬 Cutscene sona erdi.");
        OnCutsceneEnd?.Invoke();

        SceneLoader.Instance.LoadScene(sceneName, GameState.Playing);

    }
}
