using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [Header("# Main Data")]
    [SerializeField] float _maxHp = 100f;
    public float MaxHp
    {
        get => _maxHp;
    }
    [SerializeField] float _currentHp;
    public float CurrentHp
    {
        get => _currentHp;
        set => _currentHp = value;
    }
    public bool IsAlive { get; private set; }

    public event Action OnDamageTaken;
    public event Action OnDeath;

    void OnEnable()
    {
        CurrentHp = _maxHp;
        IsAlive = true;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        CurrentHp = Mathf.Clamp(CurrentHp - damage, 0, _maxHp);
        OnDamageTaken?.Invoke();

        if (0 >= CurrentHp)
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (!IsAlive) return;

        CurrentHp = Mathf.Clamp(CurrentHp + amount, 0, _maxHp);
    }

    public void SetIsAlive(bool value)
    {
        IsAlive = value;
    }
}
