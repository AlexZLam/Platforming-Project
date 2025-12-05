using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float cooldown = 0.3f;
    private float timer = 0;
    public GameObject swordBox;
    public Animator animator;
    public float destroyTime = 0.2f;

    public AudioSource audioS;
    public AudioClip death;
    public AudioClip whiff;


    [SerializeField] private GameObject worldSpawn;
    public bool dead = false;

    public DamageFeedback dmgF;

    void Update()
    {
        // Sword cooldown
        if (timer < cooldown) timer += Time.deltaTime;
        else if (Input.GetMouseButton(0) || Input.GetKeyDown("k"))
        {
            Vector3 direction = GetAttackDirection();
            Slash(direction); audioS.PlayOneShot(whiff);
            timer = 0;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            KillPlayer();
            audioS.PlayOneShot(death);
        }
    }

    public void KillPlayer()
    {
        if (!dead)
        {
            dead = true;
            audioS.PlayOneShot(death);
            dmgF.TakeDamage();
            StartCoroutine(RespawnAfterDelay(0.35f));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(delay);
        animator.SetBool("isDead", false);

        transform.position = worldSpawn.transform.position;

        if (MouseManager.Instance != null)
            MouseManager.Instance.RespawnAllMice();
        else
            Debug.LogError("MouseManager.Instance is null. Ensure a MouseManager exists in the scene.");

        dead = false;
    }

    public Vector3 GetAttackDirection()
    {
        if (Input.GetKey(KeyCode.W)) return Vector3.up;
        if (Input.GetKey(KeyCode.S)) return Vector3.down;
        if (Input.GetKey(KeyCode.A)) return Vector3.left;
        if (Input.GetKey(KeyCode.D)) return Vector3.right;
        return Vector3.right;
    }

    void Slash(Vector3 direction)
    {
        Vector3 spawnPos = transform.position + direction;
        Quaternion rotation = Quaternion.identity;

        if (direction == Vector3.up) rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector3.right) rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.down) rotation = Quaternion.Euler(0, 0, -90);
        else if (direction == Vector3.left) rotation = Quaternion.Euler(0, 0, 180);

        GameObject clone = Instantiate(swordBox, spawnPos, rotation);
        clone.transform.SetParent(this.transform, true);

        clone.GetComponent<SwordBox>().player = this;
        Destroy(clone, destroyTime);
    }
}
