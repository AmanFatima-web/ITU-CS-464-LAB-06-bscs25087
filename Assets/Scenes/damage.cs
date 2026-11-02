
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage = 10;

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
