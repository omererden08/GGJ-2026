using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Mask : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float revealDuration = 3f;
    [SerializeField] private float transitionSpeed = 5f;

    [Header("Light Colors")]
    [SerializeField] private Color revealColor = Color.white;
    [SerializeField] private Color originalColor = Color.black;

    [SerializeField] private Light2D globalLight;
    private bool isTriggered = false;

    private void Awake()
    {

        if (globalLight == null)
            Debug.LogWarning("⚠️ Global Light 2D bulunamadı!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered || globalLight == null) return;

        if (collision.CompareTag("Player"))
        {
            isTriggered = true;

            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = false;

            StartCoroutine(HandleSmoothReveal());
        }
    }


    private IEnumerator HandleSmoothReveal()
    {
        yield return StartCoroutine(SmoothColorChange(revealColor));
        yield return new WaitForSeconds(revealDuration);
        yield return StartCoroutine(SmoothColorChange(originalColor));
    }

    private IEnumerator SmoothColorChange(Color toColor)
    {
        Color fromColor = globalLight.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            globalLight.color = Color.Lerp(fromColor, toColor, t);
            yield return null;
        }

        globalLight.color = toColor; // Son renk sabitlenir
    }
}
