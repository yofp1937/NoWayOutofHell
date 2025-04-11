using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState ActiveState;
    public PatrolState PatrolState;
    
    public void Initialise()
    {
        PatrolState = new PatrolState();
        ChangeState(PatrolState);
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
