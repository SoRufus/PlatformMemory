using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputData
{
	public Vector2 Movement;
	public Vector2 Look;
	public bool Jump;
	public bool Escape;
}

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
	public InputData InputData { get; private set; } = new();
	private PlayerInput playerInput = null;

	#region Singleton
	public static InputController Instance { get { return instance; } }
	private static InputController instance;

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
		playerInput = GetComponent<PlayerInput>();
		playerInput.onActionTriggered += OnPlayerInputActionTriggered;
	}

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
		switch (context.action.name)
        {
			case "Move":
                {
					InputData.Movement = context.action.ReadValue<Vector2>();
					break;
                }
			case "Look":
				{
					InputData.Look = context.action.ReadValue<Vector2>();
					break;
				}
			case "Jump":
				{
					if (context.performed) InputData.Jump = true;
					Invoke(nameof(ResetButtons), 0.02f);
					break;
				}
			case "Escape":
				{
					if (context.performed) InputData.Escape = true;
					Invoke(nameof(ResetButtons), 0.001f);
					break;
				}
		}
    }

	private void ResetButtons()
    {
		InputData.Jump = false;
		InputData.Escape = false;
    }
}
