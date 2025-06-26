using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotHandler : MonoBehaviour, IShotHandler
{
    [Header("# External Reference Data")]
    Gun _gun;

    void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    /// <summary>
    /// 총알 발사
    /// </summary>
    public void Shot()
    {
        _gun.CanShot = false;

        // Crosshair를 바라보게 방향 전환
        Vector3 lookDir = _gun.PlayerInteract.InteractRay.direction;
        lookDir.y = 0f;
        lookDir = Quaternion.AngleAxis(30f, Vector3.up) * lookDir; // 30도 오른쪽으로 회전
        _gun.Character.forward = lookDir;

        // PoolManager를 통한 총알 발사
        ShootBullet();

        // 탄약 소비, UI 업데이트
        _gun.AmmoData.LoadedAmmo -= 1;
        _gun.PlayerUI.UpdateAmmoAction?.Invoke(_gun.GetAmmoStatus());
        
        if (_gun.AmmoData.MaxLoadedAmmo == 60) // 무기가 Scar일 경우
        {
            StartCoroutine(DelayedShootBullet(0.083f));
        }

        // 쿨타임 적용
        StartCoroutine(ShotCoolDown());
    }

    void ShootBullet()
    {
        // 1.총알 생성
        GameObject bullet = PoolManager.Instance.Get(_gun.Bullet);
        bullet.transform.position = _gun.Muzzle.position;
        bullet.transform.parent = PoolManager.Instance.transform;

        // 2.방향 계산 및 힘주기(Bullet 컴포넌트에서 실행)
        Vector3 targetPos = _gun.PlayerLook.GetCenterObjPosition(_gun.Muzzle.position);
        Vector3 shootDir = (targetPos - _gun.Muzzle.position).normalized;

        // Damage도 전달해야함
        bullet.GetComponent<Bullet>().FireToTarget(_gun.Data.Damage, _gun.AmmoData.AmmoSpeed, shootDir);
        _gun.AudioHandler.PlayShotAudio();
    }

    IEnumerator ShotCoolDown()
    {
        yield return new WaitForSeconds(_gun.Data.FireRate);
        _gun.CanShot = true;
    }

    IEnumerator DelayedShootBullet(float delay) // Scar 전용
    {
        yield return new WaitForSeconds(delay);

        ShootBullet();
        _gun.PlayerLook.Recoil(_gun.AmmoData.RecoilKickBack, _gun.AmmoData.RecoilAmount);

        // 탄약 소비, UI 업데이트
        _gun.AmmoData.LoadedAmmo -= 1;
        _gun.PlayerUI.UpdateAmmoAction?.Invoke(_gun.GetAmmoStatus());
    }
}
