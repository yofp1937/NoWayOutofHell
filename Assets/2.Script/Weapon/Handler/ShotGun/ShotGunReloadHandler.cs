using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunReloadHandler : MonoBehaviour, IReloadHandler
{
    [Header("# External Reference Data")]
    Gun _gun;
    bool _isCancledCoroutine;

    void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    public void Reload()
    {
        if (_gun.IsReloading) // 이미 장전중이면 return
        {
            Debug.LogError($"{gameObject.name} 이미 장전중입니다.");
            return;
        }
        if (_gun.AmmoData.MaxLoadedAmmo == _gun.AmmoData.LoadedAmmo) // 이미 최대치 장전중이면 return
        {
            Debug.LogError($"{gameObject.name} 총알이 가득합니다.");
            return;
        }
        if (_gun.AmmoData.RemainAmmo == 0) // 잔탄이 0발이면 return
        {
            Debug.Log($"{gameObject.name} 남은 총알이 없습니다.");
            return;
        }

        ReloadEnter();
        _gun.ReloadCoroutine = StartCoroutine(Reloading());
    }

    void ReloadEnter()
    {
        Debug.Log($"{gameObject.name} 장전 시작");
        _gun.CanShot = false;
        _gun.IsReloading = true;
        _isCancledCoroutine = false;
    }

    IEnumerator Reloading() // 장전 시작
    {
        while (_gun.AmmoData.MaxLoadedAmmo > _gun.AmmoData.LoadedAmmo && _gun.AmmoData.RemainAmmo > 0)
        {
            if (_isCancledCoroutine)
            {
                Debug.Log($"{gameObject.name}의 장전 강제 종료");
                break;
            }
            yield return new WaitForSeconds(_gun.AmmoData.ReloadTime);

            _gun.AudioHandler.PlayReloadAudio();
            _gun.AmmoData.LoadedAmmo += 1;
            _gun.AmmoData.RemainAmmo -= 1;

            _gun.PlayerUI.UpdateAmmoAction?.Invoke(_gun.GetAmmoStatus());
        }

        _gun.CanShot = true;
        _gun.IsReloading = false;
        _gun.ReloadCoroutine = null;
    }

    public void StopReloadCoroutine()
    {
        _isCancledCoroutine = true;
        StopCoroutine(_gun.ReloadCoroutine);
        _gun.IsReloading = false;
        _gun.CanShot = true;
    }
}
