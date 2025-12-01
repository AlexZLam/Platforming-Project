using UnityEngine;

public class MouseSpawnConfig : MonoBehaviour
{
    public int prefabIndex;
    public Transform pointA;
    public Transform pointB;
    public Vector3 spawnPosition;

    void OnValidate()
    {
        if (pointA != null && spawnPosition == Vector3.zero)
            spawnPosition = pointA.position;
    }

}
