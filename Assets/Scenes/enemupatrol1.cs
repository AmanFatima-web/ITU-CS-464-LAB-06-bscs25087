
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed = 2.6f;
    [SerializeField] float turnSpeed = 360f;

    Rigidbody rb;
    int index = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        if (points == null || points.Length < 2)
            return;

        rb.position = points[0].position;
        index = 1;
    }
    void FixedUpdate()
    {
        if (points == null || points.Length < 2)
            return;

        if (points[index] == null)
            return;

        Vector3 target = points[index].position;

        Vector3 currentPosition = rb.position;

        Vector3 direction = target - currentPosition;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            index = (index + 1) % points.Length;
            return;
        }

        Vector3 movement = direction.normalized * speed * Time.fixedDeltaTime;

        if (movement.magnitude > direction.magnitude)
            movement = direction;

        rb.MovePosition(currentPosition + movement);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            )
        );
    }
}
