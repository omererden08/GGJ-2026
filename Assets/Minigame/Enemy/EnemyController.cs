using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float nextNodeThreshold = 0.1f;

    [Header("Light Settings")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.black;
    [SerializeField] private float maxInterval = 3f;
    [SerializeField] private float activeDuration = 0.5f;

    private List<Transform> waypoints = new List<Transform>();
    private List<Node> currentPath = new List<Node>();
    private int currentWaypointIndex = 0;
    private int currentPathIndex = 0;

    private Rigidbody2D rb;
    private Light2D spotLight;
    private bool isFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spotLight = GetComponentInChildren<Light2D>();

        if (spotLight == null)
        {
            Debug.LogWarning("⚠️ Spot Light 2D bulunamadı!");
        }
    }

    private void Start()
    {
        // Waypoint'leri sıraya koy
        foreach (Transform child in waypointParent)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count == 0)
        {
            Debug.LogError("❌ Waypoint listesi boş!");
            enabled = false;
            return;
        }

        // Işık döngüsünü başlat
        if (spotLight != null)
        {
            StartCoroutine(LightLoop());
        }

        // Hareket döngüsünü başlat
        StartCoroutine(PathLoop());
    }

    IEnumerator PathLoop()
    {
        while (true)
        {
            if (isFrozen)
            {
                yield return null;
                continue;
            }

            Transform targetWaypoint = waypoints[currentWaypointIndex];
            currentPath = AStarPathfinder.FindPath(transform.position, targetWaypoint.position);

            if (currentPath == null || currentPath.Count == 0)
            {
                Debug.LogWarning("❌ Geçerli path bulunamadı!");
                yield break;
            }

            currentPathIndex = 0;

            while (currentPathIndex < currentPath.Count)
            {
                if (isFrozen) yield break;

                Node currentNode = currentPath[currentPathIndex];
                Vector2 direction = ((Vector2)currentNode.worldPosition - rb.position).normalized;

                // 🌀 ROTATE: yön vektöründen açıyı hesapla
                if (direction != Vector2.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    rb.rotation = angle + 180; // Rigidbody2D ile dönme
                }

                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                float distance = Vector2.Distance(rb.position, currentNode.worldPosition);
                if (distance < nextNodeThreshold)
                {
                    currentPathIndex++;
                }

                yield return new WaitForFixedUpdate();
            }


            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    IEnumerator LightLoop()
    {
        while (true)
        {
            if (isFrozen)
            {
                yield return null;
                continue;
            }

            float waitTime = Random.Range(0.5f, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (spotLight != null)
                spotLight.color = activeColor;
                AudioManager.Instance.PlaySFX(5);

            yield return new WaitForSeconds(activeDuration);

            if (spotLight != null)
                spotLight.color = inactiveColor;
                AudioManager.Instance.StopAllSFX();
        }
    }

    public void Freeze()
    {
        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 Player'a çarptım!");

            // Player'ı durdur
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.Die(); // GameManager'daki GameOver tetiklenir

            // Kendini de dondur
            Freeze();

            // Game state'i değiştir
            if (GameManager.Instance != null)
                GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}
