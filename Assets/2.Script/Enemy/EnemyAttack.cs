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
    [SerializeField] PlayerHealth _playerHealth;
    public PlayerHealth PlayerHealth { get => _playerHealth; }

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// 플레이어가 공격범위내에 들어와있는지 검사
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
                _playerHealth = hitInfo.transform.gameObject.GetComponent<PlayerHealth>();
            }
            return true;
        }
        _playerHealth = null;
        return false;
    }

    public void TakeDamage()
    {
        if (PlayerHealth == null) return;
        _playerHealth.TakeDamage(_enemy.Damage);
    }
}
