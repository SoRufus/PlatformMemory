using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseWindow = null;

    private InputController input = null;
    private GameplayManager gameplayManager = null;

    private bool isPaused = false;

    private void Start()
    {
        gameplayManager = GameplayManager.Instance;
        input = InputController.Instance;
    }

    private void Update()
    {
        InputListener();
    }

    private void InputListener()
    {
        if (!input.InputData.Escape) return;
        if (gameplayManager.ActualGameState == GameState.End) return;

        if (isPaused) ResumeGame();
        else PauseGame();
    }

    private void PauseGame()
    {
        pauseWindow.SetActive(true);
        isPaused = true;
        gameplayManager.changeGameState(GameState.Pause);
        Time.timeScale = 0;

    }

    public void ResumeGame()
    {
        pauseWindow.SetActive(false);
        isPaused = false;
        gameplayManager.changeGameState(GameState.Game);
        Time.timeScale = 1;
    }
}
