
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform player;

    [Header("Spawn Settings")]
    [SerializeField] float area = 12f;
    [SerializeField] int maxCoins = 10;
    [SerializeField] float spawnHeight = 0.5f;

    [Header("Minimum Distances")]
    [SerializeField] float minPlayerDistance = 2.5f;
    [SerializeField] float minEnemyDistance = 3f;
    [SerializeField] float minCoinDistance = 6f;

    int spawnedCoins = 0;

    Vector3 lastCoinPosition;
    bool hasPreviousCoin = false;

    void Start()
    {
        if (coinPrefab == null)
            coinPrefab = Resources.Load<GameObject>("Coin");

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
                player = obj.transform;
        }

        if (coinPrefab == null || player == null)
        {
            Debug.LogError("Coin Prefab or Player not found!");
            return;
        }

        SpawnCoin();
    }

    public void SpawnCoin()
    {
        if (spawnedCoins >= maxCoins)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-area, area),
                spawnHeight,
                Random.Range(-area, area)
            );

            if (HorizontalDistance(pos, player.position) < minPlayerDistance)
                continue;

            bool nearEnemy = false;

            foreach (GameObject enemy in enemies)
            {
                if (HorizontalDistance(pos, enemy.transform.position) < minEnemyDistance)
                {
                    nearEnemy = true;
                    break;
                }
            }

            if (nearEnemy)
                continue;

            if (hasPreviousCoin &&
                HorizontalDistance(pos, lastCoinPosition) < minCoinDistance)
                continue;

            Instantiate(coinPrefab, pos, Quaternion.identity, transform);

            lastCoinPosition = pos;
            hasPreviousCoin = true;
            spawnedCoins++;

            return;
        }

        Debug.LogWarning("No suitable coin position found. Try increasing the spawn area.");
    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;

        return Vector3.Distance(a, b);
    }
}
