using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("# Main Data")]
    PlayerInput _playerInput;
    public PlayerInput.PlayerActions Player;

    [Header("# Reference Data")]
    PlayerMovement _movement;
    PlayerLook _look;
    PlayerAnimationController _animCon;

    void Awake()
    {
        _playerInput = new PlayerInput();
        Player = _playerInput.Player;

        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        _animCon = GetComponent<PlayerAnimationController>();

        Player.Jump.performed += e => _movement.Jump();
        Player.Shot.performed += e => _animCon.Shot();

        LockCursor(true); // 시작 시 마우스 고정
    }

    void FixedUpdate()
    {
        _movement.ProcessMove(Player.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        _look.ProcessLook(Player.Look.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        _playerInput.Enable();
    }

    void OnDisable()
    {
        _playerInput.Disable();
    }

    void LockCursor(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}
