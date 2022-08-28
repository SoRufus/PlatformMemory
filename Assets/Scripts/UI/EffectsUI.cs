using UnityEngine;

public class EffectsUI : MonoBehaviour
{
    [SerializeField] private Animator gameScreenAnimator = null;

    private GameplayManager gameplayManager = null;

    private void Start()
    {
        gameplayManager = GameplayManager.Instance;
        gameplayManager.gameStateChangedEvent.AddListener(ResetGame);
    }

    private void ResetGame(GameState state)
    {
        if (state == GameState.Win) gameScreenAnimator.Play( "Win", 0);
        if (state == GameState.Lose) gameScreenAnimator.Play("Lose", 0);
    }
}
