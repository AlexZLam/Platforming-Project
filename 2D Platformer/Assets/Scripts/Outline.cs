using UnityEngine;
using System.Collections.Generic;


public class SpriteOutline : MonoBehaviour
{
    public Color outlineColor = Color.white; // Default outline color
    public float outlineSize = 0.05f;        // Distance of outline
    public bool autoContrast = true;         // Automatically pick contrasting color

    private SpriteRenderer spriteRenderer;
    private List<GameObject> outlineObjects = new List<GameObject>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If autoContrast is enabled, pick outline color based on sprite brightness
        if (autoContrast)
        {
            float brightness = (spriteRenderer.color.r + spriteRenderer.color.g + spriteRenderer.color.b) / 3f;
            outlineColor = (brightness < 0.5f) ? Color.white : Color.black;
        }

        CreateOutline();
    }

    void CreateOutline()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1,1),
            new Vector2(-1,1),
            new Vector2(1,-1),
            new Vector2(-1,-1)
        };

        foreach (Vector2 dir in directions)
        {
            GameObject outline = new GameObject("Outline");
            outline.transform.parent = transform;
            outline.transform.localPosition = dir * outlineSize;

            SpriteRenderer sr = outline.AddComponent<SpriteRenderer>();
            sr.sprite = spriteRenderer.sprite;
            sr.color = outlineColor;
            sr.sortingLayerID = spriteRenderer.sortingLayerID;
            sr.sortingOrder = spriteRenderer.sortingOrder - 1;

            outlineObjects.Add(outline);
        }
    }

    void LateUpdate()
    {
        foreach (GameObject outline in outlineObjects)
        {
            outline.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        }
    }
}
