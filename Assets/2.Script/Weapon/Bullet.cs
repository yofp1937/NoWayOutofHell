using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("# Main Data")]
    float damage;
    Vector3 _startPos;
    float _maxDistance = 150f; // 총알이 이동할 수 있는 거리
    Vector3 _lastPosition;

    [Header("# Reference Data")]
    Rigidbody _rigid;


    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 moveDir = transform.position - _lastPosition;
        float moveDis = moveDir.magnitude;

        if (moveDis > 0)
        {
            Ray ray = new Ray(_lastPosition, moveDir.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, moveDis))
            {
                if (!hit.collider.CompareTag("Weapon")) // 충돌 객체가 Weapon이 아니면 충돌처리
                {
                    HandleHit(hit.collider.gameObject, hit.point);
                }
            }
        }
    }

    /// <summary>
    /// 총알이 targetDir의 위치로 발포됨(forward, rotation은 내부에서 실행됨)
    /// </summary>
    public void FireToTarget(float damage, float bulletSpeed, Vector3 targetDir)
    {
        this.damage = damage;
        transform.forward = targetDir;
        _rigid.velocity = targetDir * bulletSpeed;

        // 최대거리 이동하면 사라지게 코루틴 설정
        _startPos = transform.position;
        _lastPosition = transform.position;
        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            float moveDis = Vector3.Distance(_startPos, transform.position);
            if (moveDis >= _maxDistance)
            {
                gameObject.SetActive(false);
                // Debug.Log($"[Bullet]: {moveDis}만큼 이동하여 Active(false)");
                yield break;
            }
            yield return null;
        }
    }

    void HandleHit(GameObject hitObj, Vector3 hitPoint)
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;

        if (hitObj.CompareTag("Enemy"))
        {
            HitBox hitBox = hitObj.GetComponent<HitBox>();
            if (hitBox != null)
            {
                float finalDamage = damage * hitBox.DamageMultiplier;
                hitObj.GetComponentInParent<Enemy>().EnemyHp.TakeDamage(finalDamage);
                Debug.Log($"충돌 부위:[{hitBox.HitPart}], 데미지: [{finalDamage}]");
            }
        }
        // 총알 위치를 충돌 지점으로 옮겨서 임팩트 효과 등 연출 가능
        transform.position = hitPoint;
        if (GameManager.Instance.DebugMode)
        {
            GameObject debugObj = PoolManager.Instance.Get(PoolManager.Instance.DebugObj);
            debugObj.transform.parent = PoolManager.Instance.DebugT;
            debugObj.transform.position = hitPoint;
        }
        gameObject.SetActive(false);
    }

}
