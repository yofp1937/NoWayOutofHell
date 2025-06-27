using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShotHandler : MonoBehaviour, IShotHandler
{
    [Header("# External Reference Data")]
    Melee _melee;

    void Awake()
    {
        _melee = GetComponent<Melee>();
    }

    /// <summary>
    /// 공격 시도
    /// </summary>
    public void Shot()
    {
        _melee.CanShot = false;
        _melee.Collider.isTrigger = true;

        // Crosshair를 바라보게 방향 전환
        Vector3 lookDir = _melee.PlayerInteract.InteractRay.direction;
        lookDir.y = 0f;
        lookDir = Quaternion.AngleAxis(17.5f, Vector3.up) * lookDir; // 15도 오른쪽으로 회전
        _melee.Character.forward = lookDir;

        // 공격 애니메이션 동작
        _melee.Player.GetComponent<PlayerAnimationController>().Anim.SetTrigger("Shot");

        // 쿨타임 적용
        StartCoroutine(ShotCoolDown());
        StartCoroutine(ColliderTriggerCoolDown());
    }

    IEnumerator ShotCoolDown()
    {
        yield return new WaitForSeconds(_melee.Data.FireRate + 0.75f);
        _melee.CanShot = true;
    }

    IEnumerator ColliderTriggerCoolDown()
    {
        yield return new WaitForSeconds(_melee.Data.FireRate);
        _melee.Collider.isTrigger = false;
        _melee.ClearHashSet();
    }
}
