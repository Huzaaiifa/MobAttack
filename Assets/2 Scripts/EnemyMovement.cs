using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private float currentZ;
    private float moveDecisionTimer = 2f;
    private float moveTimer;
    private bool shouldMove = true;

    // Smooth movement variables
    public float smoothTime = 0.5f; // Smoothing time
    private float zVelocity = 0f;   // Velocity for SmoothDamp

    void Start()
    {
        currentZ = transform.position.z;
        moveTimer = Random.Range(0f, moveDecisionTimer);
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDecisionTimer)
        {
            moveTimer = 0f;
            shouldMove = Random.value > 0.5f;
        }

        if (shouldMove)
        {
            // Horizontal movement
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // Smoothly interpolate z position
            float targetZ = Mathf.Clamp(currentZ + Random.Range(-0.25f, 0.25f), -5f, 5f);
            currentZ = Mathf.Lerp(currentZ, targetZ, smoothTime * Time.deltaTime); // Use Mathf.Lerp for smoother movement

            // Update position
            transform.position = new Vector3(transform.position.x, transform.position.y, currentZ);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit the Wall");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Collided with a non-wall object");
        }
    }
}
