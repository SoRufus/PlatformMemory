using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
	#region Singleton
	public static GameplayManager Instance { get { return instance; } }
	private static GameplayManager instance;

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

	public GameState ActualGameState { get; private set; }
	public UnityAction<GameState> changeGameState = null;

    private void Start()
    {
		ChangeGameState(GameState.Game);
    }

    private void OnEnable()
    {
		changeGameState += ChangeGameState;
    }

	private void ChangeGameState(GameState state)
    {
		switch (state)
        {
			case GameState.Game:
                {
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					break;
                }

			case GameState.Pause:
                {
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					break;
                }
		}

		ActualGameState = state;
	}
}
