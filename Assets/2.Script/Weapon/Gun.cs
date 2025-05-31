using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon, IReloadable
{
    private GunData _data => Data as GunData;
    // Data를 복사해서 사용하는 변수를 만들고 해당 변수에 접근해서 값을 변경해야함(참조 X)

    [Header("# Gun's Ammo Data")]
    public AmmoData AmmoData;
    public bool CanShot = true;

    [Header("# Gun's Reload Data")]
    public bool IsReloading = false;

    [Header("# Gun's Reference Data")]
    public GameObject Bullet;
    public Transform Muzzle; // 총구

    [Header("# Gun's Handler")]
    private ShotHandler _shotHandler;
    private ReloadHandler _reloadHandler;

    protected override void Init()
    {
        AmmoData.Clone(_data);
        _shotHandler = GetComponent<ShotHandler>();
        _reloadHandler = GetComponent<ReloadHandler>();
    }

    public virtual void Shot()
    {
        _shotHandler.Shot();

        // 발사 이후
        // 2.사운드 재생
    }

    public void Reload()
    {
        _reloadHandler.Reload();
    }

    public override void GetAmmo()
    {
        // 현재 장전된 탄환 + 남은 탄환이 최대 탄환과 같으면 return 다르면 총알 획득
        if (AmmoData.MaxAmmo == AmmoData.LoadedAmmo + AmmoData.RemainAmmo) return;
        if (AmmoData.MaxAmmo != -1)
        {
            AmmoData.RemainAmmo = AmmoData.MaxAmmo - AmmoData.LoadedAmmo;
        }
    }

    public override string GetAmmoStatus()
    {
        string result = "";

        if (_data.WeaponType == WeaponEnum.Pistol) // 권총은 총알이 무제한
        {
            result = AmmoData.LoadedAmmo.ToString();
        }
        else
        {
            result = AmmoData.LoadedAmmo.ToString() + " / " + AmmoData.RemainAmmo;
        }

        return result;
    }

    protected override void ConnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction += Shot;
        PlayerController.OnReloadAction += Reload;

        CanShot = true;
        IsReloading = false;
    }
    
    protected override void DisconnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction -= Shot;
        PlayerController.OnReloadAction -= Reload;
    }
}
