using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Enemy _enemy;
    [SerializeField] float _maxHp = 100f;
    public float CurrentHp;
    public bool IsAlive;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void Reset()
    {
        CurrentHp = _maxHp;
        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        CurrentHp -= damage;

        if (0 >= CurrentHp)
        {
            _enemy.StateMachine.ChangeState(new DeadState());
        }
    }
}
