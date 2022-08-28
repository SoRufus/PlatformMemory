using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformsManager : MonoBehaviour
{
	public UnityEvent<bool> HighLightPlatformEvent { get; private set; }

	private List<PlatformController> platformsOnScene = new();
	private List<PlatformController> correctPlatforms = new();
	private LevelManager levelManager = null;
	private int correctPlatformIndex = 0;

	#region Singleton
	public static PlatformsManager Instance { get { return instance; } }
	private static PlatformsManager instance;

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

    private void Start()
    {
		levelManager = LevelManager.Instance;
		HighLightPlatformEvent = new();
	}

	public void AddPlatform(PlatformController platform)
    {
		if (platformsOnScene.Contains(platform)) return;

		platform.transform.SetParent(transform);
		platformsOnScene.Add(platform);

		if (correctPlatforms.Contains(platform)) return;
		if (platform.Breakable) return;

		correctPlatforms.Add(platform);
    }

    public void HighLightPath()
    {
		for(int i = 0; i < correctPlatforms.Count; i++)
        {
			Invoke(nameof(HighLightPlatformAfterDelay), levelManager.GetCurrentLevelData().previewSpeed * i);
        }
	}

	private void HighLightPlatformAfterDelay()
    {
		correctPlatforms[correctPlatformIndex].Highlight();
		HighLightPlatformEvent.Invoke(correctPlatforms[correctPlatformIndex].IsLeft);
		correctPlatformIndex++;
		if (correctPlatformIndex == correctPlatforms.Count) correctPlatformIndex = 0;
    }

	private void RestartGame()
    {
		foreach(PlatformController platform in platformsOnScene)
        {
			platform.gameObject.SetActive(true);
        }
    }
}
