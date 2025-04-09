using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("# Main Data")]
    float _xRotation = 0;
    [SerializeField] float _xSensitivity = 30; // 좌우 회전속도
    [SerializeField] float _ySensitivity = 30; // 위아래 회전속도

    [Header("# Reference Data")]
    [SerializeField] Camera _cam;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // 마우스 위, 아래 이동에 따른 Rotation 계산
        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        // 카메라 위지 적용
        _cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // 마우스 왼쪽, 오른쪽 이동에 따른 Rotate 계산
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * _xSensitivity);
    }
}
