using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadHandler : MonoBehaviour, IReloadable
{
    [Header("# External Reference Data")]
    private Gun _gun;

    // 총알이 무한일때 동작방식을 수정해야함
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
        StartCoroutine(Reloading());
    }

    void ReloadEnter()
    {
        Debug.Log($"{gameObject.name} 장전 시작");
        _gun.CanShot = false;
        _gun.IsReloading = true;
    }

    IEnumerator Reloading() // 장전 시작
    {
        yield return new WaitForSeconds(_gun.AmmoData.ReloadTime);

        if (_gun.AmmoData.RemainAmmo == -1)
        {
            _gun.AmmoData.LoadedAmmo = _gun.AmmoData.MaxLoadedAmmo;
        }
        else
        {
            int _reloadAmmo = Mathf.Min(_gun.AmmoData.MaxLoadedAmmo - _gun.AmmoData.LoadedAmmo, _gun.AmmoData.RemainAmmo); // _maxLoadedAmmo를 넘기지않도록 계산
            _gun.AmmoData.LoadedAmmo += _reloadAmmo;
            _gun.AmmoData.RemainAmmo -= _reloadAmmo;
        }

        _gun.CanShot = true;
        _gun.IsReloading = false;
        _gun.PlayerUI.UpdateAmmoText(_gun.GetAmmoStatus());
        Debug.Log($"{gameObject.name} 장전 완료");
    }
}
