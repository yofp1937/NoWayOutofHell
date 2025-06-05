using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("# Main Data")]
    float damage;
    Rigidbody _rigid;
    Vector3 _startPos;
    float _maxDistance = 57f; // 총알이 이동할 수 있는 거리

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 총알이 targetDir의 위치로 발포됨
    /// </summary>
    public void FireToTarget(float damage, float bulletSpeed, Vector3 targetDir)
    {
        this.damage = damage;
        transform.forward = targetDir;
        _rigid.velocity = targetDir * bulletSpeed;

        // 57M 이동하면 사라지게 코루틴 설정
        _startPos = transform.position;
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

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObj = collision.gameObject;

        // 총알 발사하면서 총과 겹쳐져서 비활성화되지않게하는 if문
        if (!hitObj.CompareTag("Weapon"))
        {
            if (hitObj.CompareTag("Enemy"))
            {
                hitObj.GetComponent<Enemy>().TakeDamage(damage);
            }

            // Debug용 코드
            // GameObject obj = PoolManager.Instance.Get(PoolManager.Instance.DebugObj);
            // obj.transform.position = collision.contacts[0].point;
            // obj.transform.parent = PoolManager.Instance.DebugT;

            gameObject.SetActive(false);
        }
    }

}
