using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public override void Enter()
    {
        Enemy.EnemyHp.SetIsAlive(false);
        Enemy.EnemyAnimCon.SetTrigger("Dead");
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        
    }
}
