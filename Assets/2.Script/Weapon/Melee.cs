using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    private MeleeData _data => Data as MeleeData;

    [Header("# Melee's Handler")]
    [SerializeField] IShotHandler _shotHandler;
    [HideInInspector] public AudioHandler AudioHandler;
    HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();


    protected override void Init()
    {
        _shotHandler = GetComponent<IShotHandler>();
        AudioHandler = GetComponent<AudioHandler>();
    }

    public virtual void Shot()
    {
        if (!CanShot)
        {
            // Debug.LogError($"{gameObject.name}의 canShot: false");
            return;
        }

        _shotHandler.Shot();
    }

    public override void GetAmmo() // 근접 무기는 총알 보급이 필요없음
    {
        return;
    }

    public override string GetAmmoStatus()
    {
        return "1";
    }

    protected override void ConnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction += Shot;
    }

    protected override void DisconnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction -= Shot;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy == null || _hitEnemies.Contains(enemy.gameObject)) return;

        HitBox hitBox = other.GetComponent<HitBox>();
        float finalDamage = _data.Damage * hitBox.DamageMultiplier;

        enemy.EnemyHp.TakeDamage(finalDamage);
        _hitEnemies.Add(enemy.gameObject);
    }

    /// <summary>
    /// _shotHandler에서 공격이 끝나면 호출
    /// </summary>
    public void ClearHashSet()
    {
        _hitEnemies.Clear();
    }
}
