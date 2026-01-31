using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 10;

    private int currentEnemyCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies)
            return;

        Vector2 spawnPosition = GetRandomWalkablePosition();
        GameObject enemy = EnemyPool.Instance.SpawnEnemy(spawnPosition);

        if (enemy != null)
        {
            currentEnemyCount++;
            StartCoroutine(ReturnToPoolAfterDelay(enemy, 3f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy.activeInHierarchy)
        {
            EnemyPool.Instance.ReturnToPool(enemy);
            currentEnemyCount--;
        }
    }

    private Vector2 GetRandomWalkablePosition()
    {
        var allNodes = GridManager.Instance.GetAllNodes();
        var walkableNodes = new List<Node>();

        foreach (var node in allNodes)
        {
            if (node.isWalkable)
                walkableNodes.Add(node);
        }

        if (walkableNodes.Count == 0)
        {
            Debug.LogWarning("🚫 Hiç yürünebilir node yok!");
            return Vector2.zero;
        }

        Node randomNode = walkableNodes[Random.Range(0, walkableNodes.Count)];
        return randomNode.worldPosition;
    }
}
