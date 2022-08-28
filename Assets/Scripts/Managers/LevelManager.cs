using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	public List<bool> IsLeftBreakable;
}

public class LevelManager : MonoBehaviour
{
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
		Level lvl = new();
		lvl.IsLeftBreakable = new();

		for (int i = 0; i < levels[currentLevelIndex].numberOfTiles; i++)
		{
			float random = Random.value;
			lvl.IsLeftBreakable.Add(random > 0.5f);
		}

		return lvl;
	}

	private void NextLevel(GameState state)
    {
		if (state != GameState.Win) return;

		currentLevelIndex++;
		currentLevel = GenerateLevel();
		deathCounter = 0;
	}

	private void Lose(GameState state)
    {
		if (state != GameState.Lose) return;

		deathCounter++;
	}
}
