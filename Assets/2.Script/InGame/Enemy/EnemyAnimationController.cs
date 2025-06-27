using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Enemy _enemy;
    [SerializeField] Animator _anim;

    void Awake()
    {
        GetComponent();
    }

    void GetComponent()
    {
        _enemy = GetComponent<Enemy>();
        _anim = GetComponent<Animator>();
    }

    public void Reset()
    {
        if (_anim == null)
        {
            GetComponent();
        }
        _anim.SetBool("IsRun", _enemy.IsRun);
        _anim.SetBool("HasTarget", false);
    }

    public void SetFloat(string param, float value)
    {
        _anim.SetFloat(param, value);
    }

    public void SetBool(string param, bool value)
    {
        _anim.SetBool(param, value);
    }

    public void SetTrigger(string trigger)
    {
        _anim.SetTrigger(trigger);
    }
}
