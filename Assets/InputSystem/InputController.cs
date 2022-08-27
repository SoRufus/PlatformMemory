using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputData
{
	public Vector2 Movement;
	public Vector2 Look;
	public bool Jump;
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
		if (context.action.name == "Move")
        {
			InputData.Movement = context.action.ReadValue<Vector2>();
		}

		if (context.action.name == "Look")
        {
			InputData.Look = context.action.ReadValue<Vector2>();
		}

		if (context.action.name == "Jump")
        {
			InputData.Jump = true;
			Invoke(nameof(ResetJump), 0.05f);
		}
    }

	private void ResetJump()
    {
		InputData.Jump = false;
    }
}
