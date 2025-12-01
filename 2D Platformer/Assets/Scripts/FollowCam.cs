using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public Vector3 minBounds;
    public Vector3 maxBounds;

    public bool clampOn;

    public Transform target;
    public float smoothSpeed = 0.125f;
    private Vector3 smoothedPosition;

    public float moveSpeed = 2f; // manual movement speed

    private float sHoldTime = 0f;        // timer for how long S is held
    public float holdThreshold = 1f;     // seconds before movement starts
    private bool lookDownActive = false; // true once threshold reached

    public PlayerMovement pm;

    private void FixedUpdate()
    {
        // Only follow if not in look-down mode
        if (target != null && !lookDownActive)
        {
            Vector3 newPos = transform.position;
            newPos.x = target.position.x;
            newPos.y = target.position.y;

            smoothedPosition = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        }
    }

    private void LateUpdate()
    {
        if (target != null && !lookDownActive)
        {
            float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
            float clampedZ = Mathf.Clamp(smoothedPosition.z, minBounds.z, maxBounds.z);

            transform.position = clampOn
                ? new Vector3(clampedX, clampedY, clampedZ)
                : smoothedPosition;
        }
    }

    private void Update()
    {
        if (pm.isGrounded == false && pm.rb.linearVelocity.y > 0)
        {
            sHoldTime = 0f;
            lookDownActive = false;
            return; // exit Update early
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!lookDownActive)
            {
                sHoldTime += Time.deltaTime;
                if (sHoldTime >= holdThreshold)
                    lookDownActive = true;
            }

            if (lookDownActive)
            {
                // Pan down
                Vector3 newPos = transform.position + Vector3.down * moveSpeed * Time.deltaTime;

                // Clamp relative to player's current Y
                float clampedY = Mathf.Clamp(newPos.y, target.position.y - 5f, target.position.y);

                transform.position = new Vector3(newPos.x, clampedY, newPos.z);
            }
        }
        else
        {
            sHoldTime = 0f;
            lookDownActive = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 newPos = transform.position + Vector3.left * moveSpeed * Time.deltaTime;
            float clampedX = Mathf.Clamp(newPos.x, target.position.x - 5f, target.position.x + 5f);
            transform.position = new Vector3(clampedX, newPos.y, newPos.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 newPos = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
            float clampedX = Mathf.Clamp(newPos.x, target.position.x - 5f, target.position.x + 5f);
            transform.position = new Vector3(clampedX, newPos.y, newPos.z);
        }
    }
}
