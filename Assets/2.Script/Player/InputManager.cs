using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("# Main Data")]
    public bool IsGameStarted;

    [Header("# Reference Data")]
    PlayerAnimationController _animCon;
    PlayerController _playerCon;

    void Awake()
    {
        _animCon = GetComponent<PlayerAnimationController>();
        _playerCon = GetComponent<PlayerController>();
    }

    void Update()
    {
        InputMouseHandle();
        InputKeyHandle();
    }

    void InputKeyHandle()
    {
        if(!IsGameStarted) return;
    }

    void InputMouseHandle()
    {
        if(!IsGameStarted) return;
    }
}
