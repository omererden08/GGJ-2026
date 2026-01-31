using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;

    private Transform enemyParent; // Dinamik olarak oluşturulacak parent

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 🧱 Parent objesini oluştur
        GameObject parentObject = new GameObject("EnemyPool_Container");
        enemyParent = parentObject.transform;
        enemyParent.SetParent(this.transform); // İstersen pool objesinin altında dursun

        // 🪣 Enemy havuzunu oluştur
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemyParent);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject SpawnEnemy(Vector2 position)
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.transform.SetParent(enemyParent);
            enemy.transform.position = position;
            enemy.SetActive(true);
            return enemy;
        }

        Debug.LogWarning("⚠️ Enemy pool boş.");
        return null;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemy.transform.SetParent(enemyParent);
        enemyPool.Enqueue(enemy);
    }
}
