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
    PlayerItem _playerItem;

    // Delegate
    public delegate void OnShotDelegate();
    public OnShotDelegate OnShotAction;

    void Awake()
    {
        _playerInput = new PlayerInput();
        Player = _playerInput.Player;

        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        _animCon = GetComponent<PlayerAnimationController>();
        _playerItem = GetComponent<PlayerItem>();

        Player.Jump.performed += e => _movement.Jump();
        Player.Shot.performed += e => OnShotAction?.Invoke();
        Player.ChangeWeapon.performed += OnChangeWeapon;

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

    void OnChangeWeapon(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        int inputValue = int.Parse(context.control.name); // 키보드 1,2,3,4 입력 받아오기
        _playerItem.ChangeWeapon(inputValue); // PlayerItem의 ChangeWeapon 호출
    }
}
