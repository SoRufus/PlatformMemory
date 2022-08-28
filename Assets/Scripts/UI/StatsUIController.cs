using UnityEngine;
using TMPro;

public class StatsUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText = null;
    [SerializeField] private TextMeshProUGUI deathCounterText = null;

    private LevelManager levelManager = null;
    private GameplayManager gameplayManager = null;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Instance;
        gameplayManager = GameplayManager.Instance;

        SetLevelText();
        SetDeathCounterText();

        gameplayManager.gameStateChangedEvent.AddListener(NextLevel);
        gameplayManager.gameStateChangedEvent.AddListener(OnLose);
    }

    private void NextLevel(GameState state)
    {
        if (state != GameState.Win) return;

        Invoke(nameof(SetLevelText), 0.1f);
        Invoke(nameof(SetDeathCounterText), 0.1f);
    }
    private void OnLose(GameState state)
    {
        if (state != GameState.Lose) return;

        Invoke(nameof(SetDeathCounterText), 0.1f);
    }

    private void SetLevelText()
    {
        levelText.text = "Level: " + (levelManager.CurrentLevelIndex + 1).ToString();
    }

    private void SetDeathCounterText()
    {
        deathCounterText.text = "Deaths: " + levelManager.DeathCounter.ToString();
    }

}
