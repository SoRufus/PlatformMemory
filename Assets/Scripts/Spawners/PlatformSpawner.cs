using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float distanceBetweenSideTiles = 0.5f;
    [SerializeField] private float distanceBetweenFrontTiles = 0.5f;

    [Header("Other")]
    [SerializeField] private GameObject finishPrefab = null;
    [SerializeField] private GameObject platformPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    private LevelManager levelManager = null;
    private PlatformsManager platformsManager = null;
    private GameplayManager gameplayManager = null;

    private GameObject finishObject = null;

    void Start()
    {
        platformsManager = PlatformsManager.Instance;
        levelManager = LevelManager.Instance;
        gameplayManager = GameplayManager.Instance;

        gameplayManager.gameStateChangedEvent.AddListener(SpawnTiles);
        Invoke(nameof(SpawnTiles), 0.01f);
    }

    private void SpawnTiles(GameState state)
    {
        if (state != GameState.Win) return;

        Invoke(nameof(SpawnTiles),0.1f);

        if (finishObject == null) return;

        Destroy(finishObject.gameObject);
        finishObject = null;
    }

    private void SpawnTiles()
    {
        LevelData level = levelManager.GetCurrentLevelData();

        for (int i = 0; i < level.numberOfTiles; i++)
        {
            Vector3 frontTileDistance = new Vector3 (0.0f, 0.0f, spawnPoint.position.z - distanceBetweenFrontTiles * i);
            Vector3 sideTileDistance = new Vector3 (spawnPoint.position.x + distanceBetweenSideTiles, 0.0f , 0.0f);
            Vector3 platformDistance = new Vector3(2f, 4.5f, spawnPoint.position.z - (distanceBetweenFrontTiles * i - 2f));

            PlatformController rightPlatform = Instantiate(platformPrefab, spawnPoint.position + frontTileDistance, Quaternion.identity).GetComponent<PlatformController>();
            PlatformController leftPlatform = Instantiate(platformPrefab, spawnPoint.position + frontTileDistance + sideTileDistance, Quaternion.identity).GetComponent<PlatformController>();

            rightPlatform.Breakable = !levelManager.CurrentLevel.IsLeftBreakable[i];
            leftPlatform.Breakable = levelManager.CurrentLevel.IsLeftBreakable[i];

            rightPlatform.IsLeft = false;
            leftPlatform.IsLeft = true;

            platformsManager.AddPlatform(rightPlatform);
            platformsManager.AddPlatform(leftPlatform);

            if (i == level.numberOfTiles - 1) finishObject = Instantiate(finishPrefab, spawnPoint.position + platformDistance, finishPrefab.transform.rotation);
        }

        platformsManager.HighLightPath();
    }
}
