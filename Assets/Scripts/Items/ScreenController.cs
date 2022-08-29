using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private GameObject leftLight = null;
    [SerializeField] private GameObject rightLight = null;

    private PlatformsManager platformManager = null;
    private LevelManager levelManager = null;
    private GameplayManager gameplayManager = null;
    private bool isLeft = false;
    void Start()
    {
        platformManager = PlatformsManager.Instance;
        levelManager = LevelManager.Instance;
        gameplayManager = GameplayManager.Instance;


        gameplayManager.gameStateChangedEvent.AddListener(RestartLevel);
        platformManager.HighLightPlatformEvent.AddListener(ToggleLight);
    }

    private void ToggleLight(bool left)
    {
        isLeft = left;
        Invoke(nameof(ToggleLightAfterDelay), levelManager.GetCurrentLevelData().previewTime * 0.3f);
    }

    private void ToggleLightAfterDelay()
    {
        leftLight.SetActive(isLeft);
        rightLight.SetActive(!isLeft);
        float fadeDelay = levelManager.GetCurrentLevelData().previewTime * 0.9f;
        Invoke(nameof(DisableLights), fadeDelay);
    }

    private void DisableLights()
    {
        leftLight.SetActive(false);
        rightLight.SetActive(false);
    }

    private void RestartLevel(GameState state)
    {
        CancelInvoke();
        DisableLights();
    }
}
