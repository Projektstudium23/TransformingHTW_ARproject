using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    public Slider slider;
    public GameObject prefabToSpawn;
    public Transform spawnArea;

    private float minSpawnDistance = 0f;
    private float maxSpawnDistance = 10f;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private float prefabWidth = 4f;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        float spawnDistance = Mathf.Lerp(minSpawnDistance, maxSpawnDistance, value);

        int rowCapacity = Mathf.FloorToInt(spawnArea.localScale.x / prefabWidth);
        int rowCount = Mathf.CeilToInt(value / rowCapacity);

        // Entferne überschüssige Prefabs
        int prefabCount = Mathf.RoundToInt(value);
        while (spawnedPrefabs.Count > prefabCount)
        {
            GameObject prefabToRemove = spawnedPrefabs[spawnedPrefabs.Count - 1];
            spawnedPrefabs.Remove(prefabToRemove);
            Destroy(prefabToRemove);
        }

        // Spawne zusätzliche Prefabs
        while (spawnedPrefabs.Count < prefabCount)
        {
            int currentPrefabIndex = spawnedPrefabs.Count;
            int currentRow = currentPrefabIndex / rowCapacity;
            int currentColumn = currentPrefabIndex % rowCapacity;

            float offsetX = (rowCapacity * prefabWidth) / 2f - prefabWidth / 2f;
            float offsetY = spawnArea.localScale.y / 2f;

            Vector3 spawnPosition = spawnArea.position + new Vector3((currentColumn * prefabWidth) - offsetX, -offsetY - (currentRow * prefabWidth), 0f);
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedPrefabs.Add(spawnedPrefab);
        }
    }
}
