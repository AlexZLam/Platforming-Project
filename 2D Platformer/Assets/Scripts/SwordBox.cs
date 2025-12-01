using UnityEngine;

public class SwordBox : MonoBehaviour
{
    public PlayerController player;
    public float bounceForce = 5f;
    public Animator animator;

    private void Start()
    {
        animator.Play("Slash");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");

            // Check if the player was slashing downward
            if (player.GetAttackDirection() == Vector3.down)
            {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
