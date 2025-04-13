using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("# Main Data")]
    public float Hp;
    public float MoveSpeed;
    public bool IsRun;

    [Header("# State Data")]
    [SerializeField] string _currentState;
    StateMachine _stateMachine;
    public NavMeshAgent Agent;

    [Header("# Detect Variable")]
    public float SightDistance = 12.5f; // 플레이어 발견 거리
    public float FieldOfView = 60f; // 플레이어 발견 시야각
    public float EyeHeight; // 감지 높이
    
    [Header("# Reference Data")]
    public MovePath Path;
    public Animator Anim;
    GameObject _player;
    public GameObject Player { get => _player; }

    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
        _stateMachine.Initialise();
        Anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CanSeePlayer();
        _currentState = _stateMachine.ActiveState.ToString();
    }

    void OnEnable() // PoolManager에서 스폰될때마다 랜덤으로 IsRun 세팅
    {
        IsRun = Random.Range(0, 100) >= 80 ? true : false;
        Anim.SetBool("IsRun", IsRun);
        Anim.SetBool("HasTarget", false);
    }

    public bool CanSeePlayer()
    {
        if(_player != null)
        {
            // 플레이어가 시야 안에 들어와있는지 검사
            if(Vector3.Distance(transform.position, _player.transform.position) < SightDistance)
            {
                Vector3 targetDir = _player.transform.position - transform.position;
                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                if(angleToPlayer >= -FieldOfView && angleToPlayer <= FieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * EyeHeight), targetDir);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, SightDistance))
                    {
                        if(hitInfo.transform.gameObject == _player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * SightDistance);
                            Anim.SetBool("HasTarget", true);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
