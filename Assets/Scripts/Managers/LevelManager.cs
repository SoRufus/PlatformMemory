using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	public List<bool> IsLeftBreakable;
	public List<int> TurretPlacement;
}

public class LevelManager : MonoBehaviour
{
	public int MaxLevel => levels.Count;
	public int CurrentLevelIndex => currentLevelIndex;
	public int DeathCounter => deathCounter;
	public Level CurrentLevel => currentLevel;

	[SerializeField] private List<LevelData> levels = new List<LevelData>();

	private GameplayManager gameplayManager = null;
	private Level currentLevel = null;
	private int currentLevelIndex = 0;
	private int deathCounter = 0;

	#region Singleton
	public static LevelManager Instance { get { return instance; } }
	private static LevelManager instance;

	private void Awake()

	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}
    #endregion

    private void OnEnable()
    {
		currentLevel = GenerateLevel();
	}

    void Start()
    {
		gameplayManager = GameplayManager.Instance;
		gameplayManager.gameStateChangedEvent.AddListener(NextLevel);
		gameplayManager.gameStateChangedEvent.AddListener(Lose);

	}

    public LevelData GetCurrentLevelData()
    {
		return levels[currentLevelIndex];
    }

	private Level GenerateLevel()
	{
		deathCounter = 0;

		Level lvl = new();
		lvl.IsLeftBreakable = new();
		lvl.TurretPlacement = new();

		for (int i = 0; i < levels[currentLevelIndex].numberOfTiles; i++)
		{
			float random = Random.value;
			lvl.IsLeftBreakable.Add(random > 0.5f);
		}

		if(levels[currentLevelIndex].enemyTurrets > levels[currentLevelIndex].numberOfTiles)
        {
			Debug.Log("Too many turrets to spawn on this level");
			return lvl;
        }

		for (int i = 0; i < levels[CurrentLevelIndex].enemyTurrets; i++)
        {
			int turretPlacement = Random.Range(i * levels[currentLevelIndex].numberOfTiles / levels[CurrentLevelIndex].enemyTurrets, ((i + 1) * levels[currentLevelIndex].numberOfTiles) / levels[CurrentLevelIndex].enemyTurrets);
			lvl.TurretPlacement.Add(turretPlacement);
        }

		return lvl;
	}

	private void NextLevel(GameState state)
    {
		if (state != GameState.Win) return;

		currentLevelIndex++;
		currentLevel = GenerateLevel();
	}

	private void Lose(GameState state)
    {
		if (state != GameState.Lose) return;

		deathCounter++;
	}

	public void RestartGame()
    {
		currentLevelIndex = -1;
		currentLevel = null;
	}
}
