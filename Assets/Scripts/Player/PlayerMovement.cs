using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rotatingSpeed = 1;
    [SerializeField] private float walkSpeed = 30;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 maxVerticalRotation = new Vector2(-70, 70);

    [Header("Ground Check")]
    [SerializeField] private Transform floorCheckPoint = null;
    [SerializeField] private LayerMask floorLayer = default;
    [SerializeField] private float checkFloorRadius = 0.5f;

    [Header("Other")]
    [SerializeField] private Transform cameraObj = null;
    private InputController input = null;
    private GameplayManager gameplayManager = null;
    private Rigidbody rigid = null;
    private bool isReadyToJump = true;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        input = InputController.Instance;
        gameplayManager = GameplayManager.Instance;
    }

    void FixedUpdate()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (input == null) return;
        if (gameplayManager.ActualGameState != GameState.Game) return;

        Movement();
        LimitVelocity();
        Look();
        Jump();
    }

    private void Movement()
    {
        if (input.InputData.Movement.sqrMagnitude <= 0.01f) return;

        Vector3 direction = new Vector3(input.InputData.Movement.x, 0.0f, input.InputData.Movement.y).normalized;
        direction = transform.right * input.InputData.Movement.x + transform.forward * input.InputData.Movement.y;

        rigid.AddForce(direction * walkSpeed * 10f * Time.fixedDeltaTime, ForceMode.Force);
    }

    private void LimitVelocity()
    {
        Vector3 velocity = new Vector3(rigid.velocity.x, 0.0f, rigid.velocity.z);
        if (velocity.magnitude <= maxSpeed) return;

        velocity = velocity.normalized * maxSpeed;
        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
    }

    private void Jump()
    {
        if (!input.InputData.Jump) return;
        if (!IsGrounded()) return;
        if (!isReadyToJump) return;

        Vector3 direction = transform.up * jumpForce;
        rigid.AddForce(direction * jumpForce * 1f * Time.fixedDeltaTime, ForceMode.Impulse);

        isReadyToJump = false;
        Invoke(nameof(ResetJump), 0.2f);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(floorCheckPoint.position, checkFloorRadius, floorLayer, QueryTriggerInteraction.Ignore);
    }

    private void Look()
    {
        Vector2 inputRotation = input.InputData.Look;
        Vector2 camRotation = cameraObj.transform.localRotation.eulerAngles + new Vector3(-inputRotation.y, 0.0f, 0.0f) * rotatingSpeed;
        Vector2 playerRotation = transform.localRotation.eulerAngles + new Vector3(0.0f, inputRotation.x, 0.0f) * rotatingSpeed;

        camRotation.x = camRotation.x > 180 ? camRotation.x -= 360 : camRotation.x;
        camRotation.x = Mathf.Clamp(camRotation.x, maxVerticalRotation.x, maxVerticalRotation.y);

        cameraObj.transform.localRotation = Quaternion.Euler(camRotation);
        rigid.MoveRotation(Quaternion.Euler(playerRotation));
    }
}
