using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("# Main Data")]
    public float Sensitivity = 100f; // 마우스 감도

    [Header("# Reference Data")]
    [SerializeField] Transform _camArm;
    [SerializeField] Transform _upperBody;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * Sensitivity * Time.deltaTime;
        float mouseY = input.y * Sensitivity * Time.deltaTime;
        Vector3 camAngle = _camArm.rotation.eulerAngles;

        float xRotation = camAngle.x - mouseY;

        if(xRotation < 180)
        {
            xRotation = Mathf.Clamp(xRotation, -1f, 50f);
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, 335f, 361f);
        }

        _camArm.rotation = Quaternion.Euler(xRotation, camAngle.y + mouseX, camAngle.z);
    }
}
