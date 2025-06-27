using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    private GunData _data => Data as GunData;
    // Data를 복사해서 사용하는 변수를 만들고 해당 변수에 접근해서 값을 변경해야함(참조 X)

    [Header("# Gun's Shot Data")]
    Coroutine _shotCoroutine;

    [Header("# Gun's Ammo Data")]
    public AmmoData AmmoData;

    [Header("# Gun's Reload Data")]
    public bool IsReloading = false;
    public Coroutine ReloadCoroutine;

    [Header("# Gun's Reference Data")]
    public GameObject Bullet;
    public Transform Muzzle;

    [Header("# Gun's Handler")]
    [SerializeField] IShotHandler _shotHandler;
    [SerializeField] IReloadHandler _reloadHandler;
    [HideInInspector] public AudioHandler AudioHandler;

    protected override void Init()
    {
        AmmoData.Clone(_data);
        _shotHandler = GetComponent<IShotHandler>();
        _reloadHandler = GetComponent<IReloadHandler>();
        AudioHandler = GetComponent<AudioHandler>();
    }

    public virtual void Shot()
    {
        if (_shotCoroutine == null)
        {
            _shotCoroutine = StartCoroutine(ShotCoroutine());
        }
    }

    IEnumerator ShotCoroutine()
    {
        while (true)
        {
            if (ReloadCoroutine != null)
            {
                (_reloadHandler as ShotGunReloadHandler)?.StopReloadCoroutine();
                ReloadCoroutine = null;
            }
            if (!PlayerController.IsHoldingShotKey || !CanShot)
            {
                break;
            }
            if (_data.WeaponType != WeaponEnum.ShotGun && IsReloading)
            {
                break;
            }
            if (AmmoData.LoadedAmmo == 0)
            {
                Reload();
                break;
            }

            _shotHandler.Shot();
            PlayerLook.Recoil(AmmoData.RecoilKickBack, AmmoData.RecoilAmount);

            yield return new WaitForSeconds(_data.FireRate);
        }
        _shotCoroutine = null;
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
        PlayerLook.OnAimChanged += HandleAimChanged;

        CanShot = true;
        IsReloading = false;
    }

    protected override void DisconnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction -= Shot;
        PlayerController.OnReloadAction -= Reload;
        PlayerLook.OnAimChanged -= HandleAimChanged;
    }

    void HandleAimChanged(bool isAiming)
    {
        if (isAiming)
        {
            AmmoData.RecoilKickBack = new Vector3(0.025f, 0.05f, 0);
            AmmoData.RecoilAmount = 0.05f;
            // Debug.Log($"{gameObject.name}'s x: {AmmoData.RecoilKickBack.x}, y: {AmmoData.RecoilKickBack.y}, z: {AmmoData.RecoilKickBack.z}, Amount: {AmmoData.RecoilAmount}");
        }
        else
        {
            AmmoData.RecoilKickBack = _data.RecoilKickBack;
            AmmoData.RecoilAmount = _data.RecoilAmount;
            // Debug.Log($"{gameObject.name}'s x: {AmmoData.RecoilKickBack.x}, y: {AmmoData.RecoilKickBack.y}, z: {AmmoData.RecoilKickBack.z}, Amount: {AmmoData.RecoilAmount}");
        }
    }
}
