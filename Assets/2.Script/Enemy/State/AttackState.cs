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
        if(Enemy.Player) // Target 찾으면 다가가고 공격함
        {
            Vector3 playerPos = Enemy.Player.transform.position;
            // y축 방향벡터를 제거함으로써 고개를 들거나 숙이는 회전을 제거함
            Vector3 dir = (playerPos - Enemy.transform.position).normalized;
            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                Enemy.transform.rotation = targetRotation;
            }

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
