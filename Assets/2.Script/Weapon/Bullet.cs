using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("# Main Data")]
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
    public void FireToTarget(float bulletSpeed, Vector3 targetDir)
    {
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
            // 무언가랑 부딪히면 출력 후 비활성화
            Debug.Log($"[Bullet]: {collision.gameObject.name}와(과) 충돌");
            gameObject.SetActive(false);

            // TODO Enemy와 부딪히면 Enemy.TakeDamage 실행
        }
    }

}
