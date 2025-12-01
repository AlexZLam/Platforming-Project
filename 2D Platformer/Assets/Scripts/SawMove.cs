using UnityEngine;

public class sawMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;       // First point
    public Transform pointB;       // Second point
    public float speed = 2f;       // Movement speed

    private Transform targetPoint; // Current target

    void Start()
    {
        // Start moving toward point B
        targetPoint = pointB;
    }

    void Update()
    {
        // Move toward the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            // Switch target
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Touched Blade");
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.KillPlayer();   
            }
        }
    }
}
