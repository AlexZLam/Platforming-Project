using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFeedback : MonoBehaviour
{
    [Header("Object Flash")]
    private SpriteRenderer rend;
    private Color originalColor;

    [Header("Screen Flash")]
    public Image screenOverlay; // Assign a full-screen UI Image in Canvas
    public float overlayDuration = 0.3f;
    public Color overlayColor = new Color(1f, 0f, 0f, 0.5f); // semi-transparent red


    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (rend != null) originalColor = rend.material.color;

        if (screenOverlay != null)
            screenOverlay.color = new Color(0, 0, 0, 0); // start transparent
    }

    public void TakeDamage()
    {
        if (rend != null) StartCoroutine(FlashRedObject());
    }

    IEnumerator FlashRedObject()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        rend.material.color = originalColor;
    }

}
