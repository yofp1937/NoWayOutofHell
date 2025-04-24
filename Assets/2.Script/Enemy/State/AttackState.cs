using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void Enter()
    {
        Enemy.Agent.speed = Enemy.MoveSpeed;
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if(Enemy.CanSeePlayer())
        {
            Vector3 playerPos = Enemy.Player.transform.position;
            Vector3 lookTarget = new Vector3(playerPos.x, 0f, playerPos.z);
            Enemy.transform.LookAt(lookTarget);
            if(Enemy.IsInAttackRange())
            {
                Enemy.Agent.isStopped = true;
                Enemy.Anim.SetFloat("MoveSpeed", 0f);
            }
            else
            {
                Enemy.Agent.isStopped = false;
                Enemy.Anim.SetFloat("MoveSpeed", Enemy.MoveSpeed);
            }
            Enemy.Agent.SetDestination(playerPos);
        }
    }
}
