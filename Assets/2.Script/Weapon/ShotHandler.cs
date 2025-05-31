using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotHandler : MonoBehaviour, IShootable
{
    [Header("# External Reference Data")]
    private Gun _gun;

    void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    /// <summary>
    /// Player의 AmmoData에 접근해서 잔탄 확인후 발사 가능하면 총알 발사
    /// </summary>
    public void Shot()
    {
        if (!_gun.CanShot)
        {
            Debug.LogError($"{gameObject.name}의 canShot: false");
            return;
        }
        if (_gun.IsReloading)
        {
            Debug.LogError($"{gameObject.name}이 장전중");
            return;
        }
        if (0 >= _gun.AmmoData.LoadedAmmo)
        {
            Debug.LogError($"{gameObject.name}의 잔탄 0");
            return;
        }

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
        _gun.PlayerUI.UpdateAmmoText(_gun.GetAmmoStatus());

        // 쿨타임 적용
        StartCoroutine(ShotCoolDown());
    }

    private void ShootBullet()
    {
        // 1.총알 생성
        GameObject bullet = PoolManager.Instance.Get(_gun.Bullet);
        bullet.transform.position = _gun.Muzzle.position;
        bullet.transform.parent = PoolManager.Instance.transform;

        // 2.방향 계산 및 힘주기(Bullet 컴포넌트에서 실행)
        Vector3 shootDir = _gun.PlayerInteract.InteractRay.direction.normalized;
        bullet.transform.forward = shootDir;
        bullet.GetComponent<Bullet>().FireToTarget(_gun.AmmoData.BulletSpeed, shootDir);
    }

    private IEnumerator ShotCoolDown()
    {
        yield return new WaitForSeconds(_gun.Data.FireRate);
        _gun.CanShot = true;
    }
}
