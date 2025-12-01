using UnityEngine;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    [SerializeField] private List<GameObject> mousePrefabs;  // assign in Inspector
    private List<MouseSpawnConfig> configs = new List<MouseSpawnConfig>();
    private readonly List<GameObject> activeMice = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }
        Instance = this;

        // Collect all spawn configs in the scene
        configs.AddRange(FindObjectsOfType<MouseSpawnConfig>(true));
    }

    void Start()
    {
        // ? At game start: destroy any mice already in scene and respawn fresh ones
        RespawnAllMice();
    }

    public void RegisterMouse(MouseMove mouse, int type)
    {
        if (!activeMice.Contains(mouse.gameObject))
            activeMice.Add(mouse.gameObject);
    }

    public void RespawnAllMice()
    {
        //  Destroy ALL existing mice in the scene (even originals not tracked)
        var existingMice = FindObjectsOfType<MouseMove>(true);
        foreach (var mouse in existingMice)
        {
            Destroy(mouse.gameObject);
        }

        activeMice.Clear();

        // Spawn fresh from configs
        foreach (var cfg in configs)
        {
            if (cfg.prefabIndex < 0 || cfg.prefabIndex >= mousePrefabs.Count)
            {
                Debug.LogWarning($"MouseManager: Invalid prefabIndex {cfg.prefabIndex} on {cfg.name}");
                continue;
            }

            var prefab = mousePrefabs[cfg.prefabIndex];

            // Fallback: if spawnPosition not set, use pointA
            Vector3 spawnPos = (cfg.spawnPosition == Vector3.zero && cfg.pointA != null)
                               ? cfg.pointA.position
                               : cfg.spawnPosition;

            var clone = Instantiate(prefab, spawnPos, Quaternion.identity);
            var mm = clone.GetComponent<MouseMove>();
            if (mm == null)
            {
                Debug.LogError("Mouse prefab missing MouseMove component.");
                Destroy(clone);
                continue;
            }

            mm.Initialize(cfg.pointA, cfg.pointB, cfg.prefabIndex);
            activeMice.Add(clone);
        }
    }

}
