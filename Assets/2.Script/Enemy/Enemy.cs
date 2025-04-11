using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    StateMachine _stateMachine;
    NavMeshAgent _agent;
    public NavMeshAgent Agent { get => _agent; }
    [SerializeField] string _currentState;
    public MovePath Path;

    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine.Initialise();
    }
}
