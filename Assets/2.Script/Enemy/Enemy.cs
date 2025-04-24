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

    [Header("# AttackRange Variable")]
    [SerializeField] float _attackRayDistance = 1f;
    
    [Header("# Reference Data")]
    public Animator Anim;
    GameObject _player;
    public GameObject Player { get => _player; }
    PlayerHealth _playerHealth;

    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CanSeePlayer();
        _currentState = _stateMachine.ActiveState.ToString();
    }

    void OnEnable()
    {
        SetBaseData();
        Anim.SetBool("IsRun", IsRun);
        Anim.SetBool("HasTarget", false);
    }

    void SetBaseData() // 랜덤으로 IsRun 세팅
    {
        IsRun = Random.Range(0f, 100f) >= 80f;
        MoveSpeed = IsRun ? 3.5f : 1f;
        _stateMachine.Initialise();
    }

    public bool CanSeePlayer()
    {
        if(_player != null)
        {
            // 플레이어가 시야 안에 들어와있는지 검사
            if(Vector3.Distance(transform.position, _player.transform.position) < _sightDistance)
            {
                Vector3 targetDir = _player.transform.position - transform.position;
                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                if(angleToPlayer >= -_fieldOfView && angleToPlayer <= _fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * _eyeHeight), targetDir);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, _sightDistance))
                    {
                        if(hitInfo.transform.gameObject == _player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * _sightDistance);
                            Anim.SetBool("HasTarget", true);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

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

    public void TakeDamageToPlayer()
    {
        _playerHealth.TakeDamage(Damage);
    }
}
