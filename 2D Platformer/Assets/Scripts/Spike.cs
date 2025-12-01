using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public float triggerCooldown = 0.5f;
    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger && collision.CompareTag("Player"))
        {
            Debug.Log("Touched Spike");
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.KillPlayer();   // unified death handling
                StartCoroutine(SpikeCooldown());
            }
        }
    }

    private IEnumerator SpikeCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}
