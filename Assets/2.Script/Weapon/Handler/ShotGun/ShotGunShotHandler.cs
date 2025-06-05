using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunShotHandler : MonoBehaviour, IShotHandler
{
    [Header("# External Reference Data")]
    Gun _gun;
    SpreadHandler _spreadHandler;

    void Awake()
    {
        _gun = GetComponent<Gun>();
        _spreadHandler = GetComponent<SpreadHandler>();
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
        _gun.PlayerUI.UpdateAmmoText(_gun.GetAmmoStatus());

        // 쿨타임 적용
        StartCoroutine(ShotCoolDown());
    }

    private void ShootBullet()
    {
        int bulletCount = (_gun.AmmoData.MaxLoadedAmmo == 8) ? 10 : 11; // 일반 샷건이면 10발, 자동 샷건이면 11발

        List<Vector3> directions = _spreadHandler.GetSpreadDirections(bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = PoolManager.Instance.Get(_gun.Bullet);
            bullet.transform.position = _gun.Muzzle.position;
            bullet.transform.forward = _gun.Muzzle.forward;
            bullet.transform.parent = PoolManager.Instance.transform;

            bullet.transform.rotation = Quaternion.LookRotation(directions[i]);
            bullet.GetComponent<Bullet>().FireToTarget(_gun.Data.Damage, _gun.AmmoData.AmmoSpeed, directions[i]);
        }
    }

    private IEnumerator ShotCoolDown()
    {
        yield return new WaitForSeconds(_gun.Data.FireRate);
        _gun.CanShot = true;
    }
}
