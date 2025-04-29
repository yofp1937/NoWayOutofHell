using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("# Main Data")]
    public float Hp;
    public float MoveSpeed;
    public float Damage = 3f;
    public bool IsRun;

    [Header("# State Data")]
    [SerializeField] string _currentState;
    StateMachine _stateMachine;
    public NavMeshAgent Agent;

    [Header("# Detect Variable")]
    [SerializeField] float _sightDistance = 12.5f; // 플레이어 발견 거리
    [SerializeField] float _fieldOfView = 60f; // 플레이어 발견 시야각
    [SerializeField] float _eyeHeight = 1f; // 감지 높이
    float _detectCycle;
    float _detectTimer;

    [Header("# AttackRange Variable")]
    [SerializeField] float _attackRayDistance = 1f;
    
    [Header("# Reference Data")]
    public Animator Anim;
    [SerializeField] GameObject _player;
    public GameObject Player { get => _player; }
    PlayerHealth _playerHealth;

    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        _currentState = _stateMachine.ActiveState.ToString();
        if(Player == null)
        {
            FindPlayerInSight();
        }
        else
        {
            _detectTimer += Time.deltaTime;
            if(_detectTimer >= _detectCycle)
            {
                ResetDetectTimer();
                FindPlayerInSight();
            }
            DrawRayToPlayer();
        }
    }

    void OnEnable()
    {
        SetBaseData();
        Anim.SetBool("IsRun", IsRun);
        Anim.SetBool("HasTarget", false);
    }

    void SetBaseData() // 소환될때마다 기본 데이터들 리셋
    {
        _player = null;
        IsRun = Random.Range(0f, 100f) >= 80f;
        MoveSpeed = IsRun ? 3.5f : 1f;
        ResetDetectTimer();
        _stateMachine.Init();
    }

    /// <summary>
    /// 시야내 Player 검사 주기 초기화
    /// </summary>
    void ResetDetectTimer()
    {
        _detectCycle = Random.Range(3f, 10f);
        _detectTimer = 0f;
    }

    /// <summary>
    /// 시야내에서 Player를 지속적으로 검색
    /// </summary>
    void FindPlayerInSight()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _sightDistance);
        foreach(var hit in hits)
        {
            if(hit.CompareTag("Player") && IsInSightAngle(hit)) // 플레이어가 시야각 내에 들어와있는가?
            {
                _player = hit.gameObject;
                Anim.SetBool("HasTarget", true);
                break;
            }
        }
    }

    /// <summary>
    /// Debug용 Ray 그려주는 함수
    /// </summary>
    void DrawRayToPlayer()
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        Ray ray = new Ray(transform.position + (Vector3.up * _eyeHeight), targetDir);
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo, _sightDistance))
        {
            if(hitInfo.transform.gameObject == Player)
            {
                Debug.DrawRay(ray.origin, ray.direction * _sightDistance);
            }
        }
    }

    /// <summary>
    /// 시야내에 플레이어가 들어와있는지 검사
    /// </summary>
    bool IsInSightAngle(Collider col)
    {
        Vector3 targetDir = col.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, targetDir);
        return angle >= -_fieldOfView && angle <= _fieldOfView;
    }

    /// <summary>
    /// 플레이어가 공격범위내에 들어와있는지 검사
    /// </summary>
    public bool IsInAttackRange()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * 1f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _attackRayDistance, Color.red);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, _attackRayDistance))
        {
            if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _playerHealth = hitInfo.transform.gameObject.GetComponent<PlayerHealth>();
            }
            return true;
        }
        return false;
    }

    // 해당 함수는 Animation 탭의 Timeline에서 Add Event로 추가하여 사용중
    public void TakeDamageToPlayer()
    {
        _playerHealth.TakeDamage(Damage);
    }
}
