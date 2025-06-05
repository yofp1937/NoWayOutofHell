using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadHandler : MonoBehaviour
{
    [Header("# Main Data")]
    [SerializeField] float _spreadAngle = 7.5f;

    [Header("# Reference Data")]
    Gun _gun;

    void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    public List<Vector3> GetSpreadDirections(int bulletCount)
    {
        List<Vector3> directions = new List<Vector3>();
        Vector3 baseDir = _gun.PlayerInteract.InteractRay.direction;

        for (int i = 0; i < bulletCount; i++)
        {
            float yaw = Random.Range(-_spreadAngle, _spreadAngle); // 좌우 퍼짐
            float pitch = Random.Range(-_spreadAngle, _spreadAngle); // 상하 퍼짐
            Quaternion spreadRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 spreadDir = spreadRotation * baseDir;

            directions.Add(spreadDir.normalized);
        }

        return directions;
    }
}