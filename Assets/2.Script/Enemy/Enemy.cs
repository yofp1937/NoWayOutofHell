using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("# Main Data")]
    public float Damage;
    public float MoveSpeed;
    public bool IsRun;

    [Header("# State Data")]
    [SerializeField] string _currentState;
    public StateMachine StateMachine;

    [Header("# Reference Data")]
    [HideInInspector] public EnemyHealth EnemyHealth;
    [HideInInspector] public EnemyAttack EnemyAttack;
    [HideInInspector] public EnemyAnimationController EnemyAnimCon;

    [Header("# External Reference Data")]
    TargetScanner _scanner;
    GameObject _player;
    public GameObject Player { get => _player; }
    public NavMeshAgent Agent;

    void Awake()
    {
        StateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
        _scanner = GetComponent<TargetScanner>();

        EnemyHealth = GetComponent<EnemyHealth>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyAnimCon = GetComponent<EnemyAnimationController>();
    }

    void OnEnable()
    {
        SetMainData();
        _scanner.ResetDetectTimer();
        StateMachine.Init();
        EnemyHealth.Reset();
        EnemyAnimCon.Reset();
    }

    void SetMainData() // 소환될때마다 기본 데이터들 리셋
    {
        _player = null;
        IsRun = Random.Range(0f, 100f) >= 80f; // 20% 확률로 뛰는 좀비
        MoveSpeed = IsRun ? 3.5f : 1f;
    }

    public void SetTarget(GameObject target)
    {
        _player = target;
        EnemyAnimCon.SetBool("HasTarget", true);
    }

    // EnemySpawnTrigger에서 Wave 트리거에의해 좀비를 소환할때마다 호출됨
    public void SetTargetFromEvent(GameObject player)
    {
        _player = player;
        _scanner.SetIsWaved();
        EnemyAnimCon.SetBool("HasTarget", true);
    }

    // 해당 함수는 Animation 탭의 Z_Attack 애니메이션의 Timeline에서 Add Event로 추가하여 사용중
    public void TakeDamageToPlayer()
    {
        EnemyAttack.TakeDamage();
    }

    // 해당 함수는 Animation 탭의 Z_FallingBack, Z_FallingForward 두 애니메이션의 Timeline에서 Add Event로 추가하여 사용중
    public void Dead()
    {
        gameObject.SetActive(false);
    }

    public void UpdateCurrentState(string state)
    {
        _currentState = state;
    }
}
