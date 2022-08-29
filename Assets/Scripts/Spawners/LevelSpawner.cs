using UnityEngine;
using System.Collections.Generic;

public class LevelSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float distanceBetweenSideTiles = 0.5f;
    [SerializeField] private float distanceBetweenFrontTiles = 0.5f;

    [Header("Prefabs")]
    [SerializeField] private GameObject finishPrefab = null;
    [SerializeField] private GameObject platformPrefab = null;
    [SerializeField] private GameObject turretPrefab = null;

    [Header("Other")]
    [SerializeField] private Transform spawnPoint = null;

    private LevelManager levelManager = null;
    private PlatformsManager platformsManager = null;
    private GameplayManager gameplayManager = null;

    private List<GameObject> turretObjects = new();
    private GameObject finishObject = null;

    void Start()
    {
        platformsManager = PlatformsManager.Instance;
        levelManager = LevelManager.Instance;
        gameplayManager = GameplayManager.Instance;

        gameplayManager.gameStateChangedEvent.AddListener(Spawn);
        Invoke(nameof(Spawn), 0.01f);
    }

    private void Spawn(GameState state)
    {
        if (state != GameState.Win) return;

        Invoke(nameof(Spawn),0.1f);

        foreach (GameObject turret in turretObjects)
        {
            Destroy(turret);
        }
        Destroy(finishObject.gameObject);
        turretObjects.Clear();
        finishObject = null;
    }

    private void Spawn()
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

        for (int i = 0; i < level.enemyTurrets; i++)
        {
            Vector3 frontTileDistance = new Vector3(0.0f, 0.0f, spawnPoint.position.z - distanceBetweenFrontTiles * levelManager.CurrentLevel.TurretPlacement[i]);
            Vector3 sideTileDistance = new Vector3(spawnPoint.position.x + distanceBetweenSideTiles * 4, 0.0f, 0.0f);
            GameObject turret = Instantiate(turretPrefab, spawnPoint.position + frontTileDistance + sideTileDistance, Quaternion.identity);

            turretObjects.Add(turret);
        }

        platformsManager.HighLightPath();
    }
}
