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
        Vector3 targetpos = _gun.PlayerInteract.InteractRay.direction.normalized;

        for (int i = 0; i < bulletCount; i++)
        {
            // 1. 무작위 방향 생성 (2D 원형 분포)
            float angle = Random.Range(0f, 360f);        // 방향 (도)
            float radius = Random.Range(0f, 1f);         // 거리 (0~1 사이)
            float spreadRadius = Mathf.Tan(_spreadAngle * Mathf.Deg2Rad); // 각도를 거리로 변환

            // 2. 평면에서 방향 벡터 생성
            Vector3 randomOffset = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f
            ) * radius * spreadRadius;

            // 3. 기본 방향을 기준으로 좌표계 회전
            Vector3 spreadDir = Quaternion.LookRotation(targetpos) * (Vector3.forward + randomOffset);
            directions.Add(spreadDir.normalized);
        }

        return directions;
    }
}