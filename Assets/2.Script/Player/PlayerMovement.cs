using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("# Main Data")]
    [SerializeField] float _moveSpeed = 2.75f;
    Vector3 _playerVelocity;

    [Header("# Jump")]
    bool _isGrounded;
    [SerializeField] float _gravity = -50f;
    [SerializeField] float _jumpHeight = 1f;

    [Header("# Reference Data")]
    [SerializeField] Transform _camArm;
    [SerializeField] Transform _character;
    CharacterController _controller;
    PlayerAnimationController _animCon;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animCon = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;
        // 좌,우 방향키를 반복적으로 누르면 Chracter가 화면에서 점점 밀려나서 Player와 Character의 position을 일치시킴
        _character.position = gameObject.transform.position;
    }

    public void ProcessMove(Vector2 input)
    {
        // 조작키로 x축, z축 이동
        Vector3 camForward = _camArm.forward;
        Vector3 camRight = _camArm.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;
        moveDir.Normalize();

        // 뒤쪽 이동이면 속도 33%로 조정
        float forwardAmount = Vector3.Dot(moveDir, camForward);
        float speedMultiplier = (forwardAmount < -0.1f) ? 0.5f : 1f;

        _controller.Move(moveDir * _moveSpeed * speedMultiplier * Time.deltaTime);

        // characet 객체도 회전 - 플레이어가 방향키 입력방향을 쳐다봄
        // if (moveDir.magnitude >= 0.1f)
        // {
        //     Quaternion playerRotation = Quaternion.LookRotation(moveDir);
        //     Vector3 euler = playerRotation.eulerAngles;
        //     _character.rotation = Quaternion.Slerp(_character.rotation, Quaternion.Euler(0, euler.y + 30f, 0), Time.deltaTime * 10f);
        // }

        // 플레이어가 언제나 정면을 쳐다봄
        Vector3 lookDir = _camArm.forward;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            // 우측으로 45도 회전 추가
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            targetRotation *= Quaternion.Euler(0f, 45f, 0f);

            _character.rotation = Quaternion.Slerp(_character.rotation, targetRotation, Time.deltaTime * 10f);
        }

        Vector3 localMove = _character.InverseTransformDirection(moveDir.normalized);
        _animCon.SetMoveDirection(localMove);

        // 점프로 y축 이동
        _playerVelocity.y += _gravity * Time.deltaTime;
        if(_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = -2f;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if(_isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
        }
    }
}
