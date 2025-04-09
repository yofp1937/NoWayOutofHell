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
    [SerializeField] float _gravity = -50;
    [SerializeField] float _jumpHeight = 1;

    [Header("# Reference Data")]
    CharacterController _controller;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        // 조작키로 x축, z축 이동
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        _controller.Move(transform.TransformDirection(moveDir) * _moveSpeed * Time.deltaTime);

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
