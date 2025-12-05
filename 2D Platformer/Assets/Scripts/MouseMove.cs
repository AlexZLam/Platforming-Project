using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public Animator animator;

    private Vector3 target;
    private bool canTrigger = true;
    private float triggerCooldown = 0.5f;
    private bool activeMouse = true;
    private int mouseType;
    private bool isAlive = true;

    public AudioSource audioS;
    public AudioClip death;

    public void Initialize(Transform a, Transform b, int type)
    {
        pointA = a;
        pointB = b;
        mouseType = type;
        target = pointB.position;
        activeMouse = true;

        if (animator != null) animator.SetBool("isDead", false);

        // Register with manager
        if (MouseManager.Instance != null)
            MouseManager.Instance.RegisterMouse(this, mouseType);
    }

    void Update()
    { 
        if (!activeMouse) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
            target = (target == pointA.position) ? pointB.position : pointA.position;

        GetComponent<SpriteRenderer>().flipX = (target == pointB.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger && collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.KillPlayer();
                StartCoroutine(MouseCooldown());
            }
        }

        if (collision.CompareTag("Sword"))
        {
            StartCoroutine(Die());
            audioS.PlayOneShot(death);
        }
    }

    private IEnumerator MouseCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }

    private IEnumerator Die()
    {
        activeMouse = false;

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(0.3f); // safer fixed delay
        }
        else yield return new WaitForSeconds(0.3f);

        gameObject.SetActive(false); // disable until respawn
    }
}
