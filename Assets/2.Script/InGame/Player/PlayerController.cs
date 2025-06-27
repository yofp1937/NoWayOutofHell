using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("# Main Data")]
    PlayerInput _playerInput;
    public PlayerInput.PlayerActions Player;
    public bool IsHoldingShotKey;

    [Header("# Reference Data")]
    Player _player;

    // Action
    public Action OnShotAction;
    public Action OnReloadAction;

    void Awake()
    {
        _playerInput = new PlayerInput();
        Player = _playerInput.Player;
        _player = GetComponent<Player>();

        Player.Jump.performed += e => _player.PlayerMovement.Jump();
        Player.ChangeWeapon.performed += OnChangeWeapon;
        Player.Reload.performed += e => OnReloadAction?.Invoke();
        Player.Aiming.performed += e => _player.PlayerLook.Aiming();

        LockCursor(true); // 시작 시 마우스 고정
    }

    void Update()
    {
        IsHoldingShotKey = Player.Shot.ReadValue<float>() > 0 ? true : false;

        if (IsHoldingShotKey)
        {
            OnShotAction?.Invoke();
        }
    }

    void FixedUpdate()
    {
        _player.PlayerMovement.ProcessMove(Player.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        _player.PlayerLook.ProcessLook(Player.Look.ReadValue<Vector2>());
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

    void OnChangeWeapon(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        int inputValue = int.Parse(context.control.name); // 키보드 1,2,3,4 입력 받아오기
        _player.PlayerItem.ChangeWeapon(inputValue); // PlayerItem의 ChangeWeapon 호출
    }
}
