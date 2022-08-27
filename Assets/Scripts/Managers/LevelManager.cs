using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

	[SerializeField] private List<LevelData> levels = new List<LevelData>();
	private int currentLevel = 0;

	public LevelData GetCurrentLevel()
    {
		return levels[currentLevel];
    }

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


}
