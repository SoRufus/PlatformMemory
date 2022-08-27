using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float distanceBetweenSideTiles = 0.5f;
    [SerializeField] private float distanceBetweenFrontTiles = 0.5f;

    [Header("Other")]
    [SerializeField] private PlatformController platformPrefab = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private GameObject spawnedPlatformsContainer = null;

    private LevelManager levelManager = null;

    void Start()
    {
        levelManager = LevelManager.Instance;
        SpawnTiles();
    }

    private void SpawnTiles()
    {
        LevelData level = levelManager.GetCurrentLevel();

        for (int i = 0; i < level.numberOfTiles; i++)
        {
            Vector3 frontTileDistance = new Vector3(0.0f, 0.0f, spawnPoint.position.z - distanceBetweenFrontTiles * i);
            Vector3 sideTileDistance = new Vector3(spawnPoint.position.x + distanceBetweenSideTiles, 0.0f , 0.0f);

            PlatformController rightPlatform = Instantiate(platformPrefab.gameObject, spawnPoint.position + frontTileDistance, Quaternion.identity).GetComponent<PlatformController>();
            PlatformController leftPlatform = Instantiate(platformPrefab.gameObject, spawnPoint.position + frontTileDistance + sideTileDistance, Quaternion.identity).GetComponent<PlatformController>();

            if (Random.value > 0.5) rightPlatform.Breakable = true;
            else leftPlatform.Breakable = true;

            rightPlatform.transform.SetParent(spawnedPlatformsContainer.transform);
            leftPlatform.transform.SetParent(spawnedPlatformsContainer.transform);
        }
    }
}
