using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("# Main Data")]
    public float Sensitivity = 30f; // 마우스 감도
    [SerializeField] float _maxVerticalAngle = 60f; // 위로 볼수있는 최대 각도
    [SerializeField] float _minVerticalAngle = 325f; // 아래로 볼수있는 최대 각도

    [Header("# External Reference Data")]
    [SerializeField] Transform _camArm;
    [SerializeField] Transform _upperBody;
    [SerializeField] RecoilHandler _recoilHandler;

    void Update()
    {
        _recoilHandler.RecoilBack();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * Sensitivity * Time.deltaTime;
        float mouseY = input.y * Sensitivity * Time.deltaTime;
        Vector3 camAngle = _camArm.rotation.eulerAngles;

        float xRotation = camAngle.x - mouseY;

        if (xRotation < 180)
        {
            xRotation = Mathf.Clamp(xRotation, -1f, _maxVerticalAngle);
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, _minVerticalAngle, 361f);
        }

        _camArm.rotation = Quaternion.Euler(xRotation, camAngle.y + mouseX, camAngle.z);
    }

    public void Recoil(Vector3 recoilKickBack, float recoilAmount)
    {
        _recoilHandler.Recoil(recoilKickBack, recoilAmount);
    }
}