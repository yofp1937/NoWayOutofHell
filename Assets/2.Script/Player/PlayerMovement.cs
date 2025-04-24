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
        _controller.Move(moveDir * _moveSpeed * Time.deltaTime);

        // characet 객체도 회전
        if(moveDir.magnitude >= 0.1f)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDir);
            Vector3 euler = playerRotation.eulerAngles;
            _character.rotation = Quaternion.Slerp(_character.rotation, Quaternion.Euler(0, euler.y + 40f, 0), Time.deltaTime * 10f);
            _animCon.SetMove(true);
        }
        else
        {
            _animCon.SetMove(false);
        }

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
