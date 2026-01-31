using System.Collections;
using UnityEngine;

public class EnemySpawnWarning : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject enemyObject;              // Asıl enemy objesi (child)
    public GameObject warningObject;            // Yanıp sönecek sprite objesi (child)
    public float warningDuration = 1.5f;        // Toplam uyarı süresi
    public float blinkInterval = 0.2f;          // Yanıp sönme hızı

    private void OnEnable()
    {
        StartCoroutine(SpawnSequence());
    }

    private IEnumerator SpawnSequence()
    {
        float timer = 0f;
        bool isVisible = false;

        SpriteRenderer sr = warningObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("⚠️ SpriteRenderer eksik!");
            yield break;
        }

        warningObject.SetActive(true);
        enemyObject.SetActive(false);

        while (timer < warningDuration)
        {
            isVisible = !isVisible;
            sr.enabled = isVisible;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        sr.enabled = false;
        warningObject.SetActive(false);
        enemyObject.SetActive(true); // Enemy sahneye girer
    }
}
