using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("# Main Data")]
    public float Sensitivity = 30f; // 마우스 감도
    [SerializeField] float _maxVerticalAngle = 60f; // 위로 볼수있는 최대 각도
    [SerializeField] float _minVerticalAngle = 325f; // 아래로 볼수있는 최대 각도

    [Header("# Reference Data")]
    Player _player;
    [SerializeField] RecoilHandler _recoilHandler;

    [Header("# Aiming")]
    public bool IsAiming;
    public Action<bool> OnAimChanged;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        _recoilHandler = _player.ObjectArm.GetComponentInChildren<RecoilHandler>();
    }

    void Update()
    {
        _recoilHandler.RecoilBack();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * Sensitivity * Time.deltaTime;
        float mouseY = input.y * Sensitivity * Time.deltaTime;
        Vector3 camAngle = _player.ObjectArm.rotation.eulerAngles;

        float xRotation = camAngle.x - mouseY;

        if (xRotation < 180)
        {
            xRotation = Mathf.Clamp(xRotation, -1f, _maxVerticalAngle);
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, _minVerticalAngle, 361f);
        }

        _player.ObjectArm.rotation = Quaternion.Euler(xRotation, camAngle.y + mouseX, camAngle.z);
    }

    public void Recoil(Vector3 recoilKickBack, float recoilAmount)
    {
        _recoilHandler.Recoil(recoilKickBack, recoilAmount);
    }

    public void Aiming()
    {
        IsAiming = !IsAiming;

        _player.Camera.fieldOfView = IsAiming ? 20 : 60;
        OnAimChanged?.Invoke(IsAiming);
    }

    public Vector3 GetCenterObjPosition(Vector3 baseT)
    {
        Ray ray = _player.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        float maxDistance = 150f;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxDistance;
        }
        return targetPoint;
    }
}