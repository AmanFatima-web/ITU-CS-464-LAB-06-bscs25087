
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    [Header("Knockback Settings")]
    [SerializeField] float knockbackSpeed = 8f;
    [SerializeField] float knockbackDuration = 0.2f;

    Rigidbody rb;
    Vector3 input;
    Vector3 knockbackDirection;

    float speedMultiplier = 1f;
    float knockbackTimer = 0f;

    Coroutine boost;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        input = Vector3.ClampMagnitude(
            new Vector3(h, 0f, v), 1f
        );
    }

    void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            rb.linearVelocity = new Vector3(
                knockbackDirection.x * knockbackSpeed,
                rb.linearVelocity.y,
                knockbackDirection.z * knockbackSpeed
            );

            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector3 movement = input * moveSpeed * speedMultiplier;

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 direction =
                transform.position - collision.transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                knockbackDirection = direction.normalized;
                knockbackTimer = knockbackDuration;
            }
        }
    }

    public void BoostSpeed(float multiplier, float seconds)
    {
        if (boost != null)
            StopCoroutine(boost);

        boost = StartCoroutine(Boost(multiplier, seconds));
    }

    IEnumerator Boost(float multiplier, float seconds)
    {
        speedMultiplier = multiplier;

        yield return new WaitForSeconds(seconds);

        speedMultiplier = 1f;
        boost = null;
    }
}
