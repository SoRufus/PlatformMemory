using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject endWindow = null;

    private GameplayManager gameplayManager = null;
    private LevelManager levelManager = null;

    private void Start()
    {
        gameplayManager = GameplayManager.Instance;
        levelManager = LevelManager.Instance;

        gameplayManager.gameStateChangedEvent.AddListener(EndGame);
    }

    private void EndGame(GameState state)
    {
        if (state != GameState.End) return;

        endWindow.SetActive(true);

        Time.timeScale = 0;  
    }

    public void RestartGame()
    {
        endWindow.SetActive(false);
        levelManager.RestartGame();
        gameplayManager.changeGameState(GameState.Win);
        Time.timeScale = 1;
    }
}
