using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    public PlayerInput.PlayerActions player;

    PlayerMovement movement;
    PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;

        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

        player.Jump.performed += e => movement.Jump();
    }

    void FixedUpdate()
    {
        movement.ProcessMove(player.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(player.Look.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
}
