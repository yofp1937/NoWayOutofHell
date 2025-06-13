using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    [Header("# Detect Variable")]
    [SerializeField] float _sightDistance = 10f; // 플레이어 발견 거리
    [SerializeField] float _fieldOfView = 60f; // 플레이어 발견 시야각
    [SerializeField] float _eyeHeight = 1f; // 감지 높이
    float _detectCycle;
    float _detectTimer;
    bool _isWaved;

    [Header("# Reference Data")]
    Enemy _enemy;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (!_isWaved) // Wave가 아니면 Player 검색을 시도함
        {
            if (_enemy.Player == null) // Player가 null이면 계속 감지 시도
            {
                FindPlayerInSight();
            }
            else // Player 발견시 3~10초마다 감지 시도(Player와 멀어지면 추적 중지)
            {
                _detectTimer += Time.deltaTime;
                if (_detectTimer >= _detectCycle)
                {
                    ResetDetectTimer();
                    FindPlayerInSight();
                }
            }
        }

        if (_enemy.Player != null)
        {
            DrawRayToPlayer();
        }
    }

    /// <summary>
    /// SpawnTrigger로 소환됐을때 호출
    /// </summary>
    public void SetIsWaved()
    {
        _isWaved = true;
        _enemy.StateMachine.ChangeState(new AttackState());
    }

    /// <summary>
    /// 시야내 Player 검사 주기 초기화
    /// </summary>
    public void ResetDetectTimer()
    {
        _detectCycle = Random.Range(3f, 10f);
        _detectTimer = 0f;
    }

    /// <summary>
    /// 시야내에서 Player를 지속적으로 검색
    /// 만약 플레이어가 없다면 player는 null로 설정됨
    /// </summary>
    public void FindPlayerInSight()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _sightDistance);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && IsInSightAngle(hit)) // 플레이어가 시야각 내에 들어와있는가?
            {
                _enemy.SetTarget(hit.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// 시야내에 플레이어가 들어와있는지 bool로 return
    /// </summary>
    bool IsInSightAngle(Collider col)
    {
        Vector3 targetDir = col.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, targetDir);
        return angle >= -_fieldOfView && angle <= _fieldOfView;
    }

    /// <summary>
    /// Debug용 Ray 그려주는 함수
    /// </summary>
    void DrawRayToPlayer()
    {
        Vector3 targetDir = _enemy.Player.transform.position - transform.position;
        Ray ray = new Ray(transform.position + (Vector3.up * _eyeHeight), targetDir);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, _sightDistance))
        {
            if (hitInfo.transform.gameObject == _enemy.Player)
            {
                Debug.DrawRay(ray.origin, ray.direction * _sightDistance);
            }
        }
    }
}
