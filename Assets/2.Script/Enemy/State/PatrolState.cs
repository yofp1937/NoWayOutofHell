using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    float _patrolRadius = 7.5f;
    float _randomTime;
    float _waitTimer;

    public override void Enter()
    {
        Enemy.Agent.speed = Enemy.MoveSpeed;
    }

    public override void Perform()
    {
        PatrolCycle();
        if(Enemy.Player) // Target 생기면 AttackState로 전환
        {
            StateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        if(Enemy.Agent.remainingDistance < 0.2f) // 목표 거리에 가까워져있으면
        {
            Enemy.Anim.SetFloat("MoveSpeed", 0f);
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _randomTime)
            {
                if(Random.Range(0,100f) >= 10f) // 20% 확률로 이동 
                {
                    Enemy.Anim.SetFloat("MoveSpeed", Enemy.Agent.speed);
                    Enemy.Agent.SetDestination(Enemy.transform.position + (Vector3)(Random.insideUnitCircle * _patrolRadius));
                }
                ResetTimer();
            }
        }
    }
    
    void ResetTimer()
    {
        _waitTimer = 0;
        _randomTime = Random.Range(5f, 16f);
    }
}
