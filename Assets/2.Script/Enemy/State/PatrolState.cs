using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int WaypointIndex;
    float _randomTime;
    float _waitTimer;

    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        PatrolCycle();
    }

    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        if(Enemy.Agent.remainingDistance < 0.2f) // 목표 거리에 가까워져있으면
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _randomTime)
            {
                if(Random.Range(0,100) >= 80) // 20% 확률로 이동 
                {
                    WaypointIndex = Random.Range(0, Enemy.Path.Waypoints.Count);
                    Enemy.Agent.SetDestination(Enemy.Path.Waypoints[WaypointIndex].position);
                }
                ResetTimer();
            }
        }
    }
    
    void ResetTimer()
    {
        _waitTimer = 0;
        _randomTime = Random.Range(5, 16);
    }
}
