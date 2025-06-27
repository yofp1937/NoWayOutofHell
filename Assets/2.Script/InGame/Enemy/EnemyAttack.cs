using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("# Main Data")]
    float _attackRayDistance = 1f;

    [Header("# Reference Data")]
    Enemy _enemy;

    [Header("# External Reference Data")]
    [SerializeField] Hp _playerHp;
    public Hp PlayerHp { get => _playerHp; }

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// 플레이어가 공격범위내에 들어와있는지 검사후 PlayerHp 할당
    /// </summary>
    public bool IsInAttackRange()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * 1f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _attackRayDistance, Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _attackRayDistance))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _playerHp = hitInfo.transform.gameObject.GetComponent<Hp>();
            }
            return true;
        }
        _playerHp = null;
        return false;
    }

    public void TakeDamage()
    {
        if (PlayerHp == null) return;
        _playerHp.TakeDamage(_enemy.Damage);
    }
}
