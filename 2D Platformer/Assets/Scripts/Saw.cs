using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour
{
    public float triggerCooldown = 0.5f;
    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger && collision.CompareTag("Player"))
        {
            Debug.Log("Touched Blade");
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.KillPlayer();   
                StartCoroutine(SawCooldown());
            }
        }
    }

    private IEnumerator SawCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}
