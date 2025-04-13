using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState ActiveState;
    
    public void Initialise()
    {
        ChangeState(new PatrolState());
    }

    void Update()
    {
        if(ActiveState != null)
        {
            ActiveState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if(ActiveState != null)
        {
            ActiveState.Exit();
        }
        ActiveState = newState;

        if(ActiveState != null)
        {
            ActiveState.StateMachine = this;
            ActiveState.Enemy = GetComponent<Enemy>();
            ActiveState.Enter();
        }
    }
}
