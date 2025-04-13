using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void Enter()
    {
        if(Enemy.IsRun)
        {
            Enemy.Agent.speed = 2.5f;
        }
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if(Enemy.CanSeePlayer())
        {
            Vector3 playerPos = Enemy.Player.transform.position;
            Enemy.transform.LookAt(playerPos);
            Enemy.Anim.SetFloat("MoveSpeed", Enemy.Agent.speed);
            Enemy.Agent.SetDestination(playerPos);
        }
    }
}
