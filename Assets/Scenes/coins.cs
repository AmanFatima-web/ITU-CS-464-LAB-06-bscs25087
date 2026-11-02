
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(value);

        Spawner spawner = FindFirstObjectByType<Spawner>();

        if (spawner != null)
            spawner.SpawnCoin();

        Destroy(gameObject);
    }
}
