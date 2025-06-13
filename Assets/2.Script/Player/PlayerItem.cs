using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [Header("# Player Equips")]
    public Weapon MainWeapon; // 라이플, 샷건, 스나이퍼
    public Weapon SubWeapon; // 피스톨, 근접무기
    public Weapon ThrowingWeapon; // 투척무기
    public Weapon HealItem; // 회복용 아이템

    [Header("# Equips Transform")]
    public Transform MainT;
    public Transform[] SubT; // 0은 Pistol, 1은 Melee
    public Transform ThrowingT;
    public Transform HealT;

    [Header("# Reference Data")]
    Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();

        MainT.gameObject.SetActive(false);
    }

    /// <summary>
    /// 무기 획득시 호출
    /// </summary>
    public void GetWeapon(Weapon weapon)
    {
        switch (weapon.Data.WeaponType)
        {
            case WeaponEnum.Rifle:
            case WeaponEnum.ShotGun:
                ChangeEquippedWeapon(ref MainWeapon, weapon);
                ChangeWeapon(1);
                weapon.SetPosition(MainT);
                break;
            case WeaponEnum.Pistol:
            case WeaponEnum.Melee:
                ChangeEquippedWeapon(ref SubWeapon, weapon);
                ChangeWeapon(2);
                if (weapon.Data.WeaponType == WeaponEnum.Pistol)
                    weapon.SetPosition(SubT[0]);
                else
                    weapon.SetPosition(SubT[1]);
                break;
            case WeaponEnum.ThrowingWeapon:
                ChangeEquippedWeapon(ref ThrowingWeapon, weapon);
                ChangeWeapon(3);
                weapon.SetPosition(ThrowingT);
                break;
            case WeaponEnum.HealItem:
                ChangeEquippedWeapon(ref HealItem, weapon);
                ChangeWeapon(4);
                weapon.SetPosition(HealT);
                break;
        }
    }

    public void ChangeWeapon(int num) // 이건 1번, 2번, 3번, 4번을 눌러서 내가 들고있는 무기의 상태가 바뀌면 호출
    {
        Weapon target = null;
        Transform targetTransform = null;
        switch (num)
        {
            case 1:
                if (MainWeapon == null) return;
                target = MainWeapon;
                targetTransform = MainT;
                break;
            case 2:
                if (SubWeapon == null) return;
                target = SubWeapon;
                targetTransform = SubWeapon.Data.WeaponType == WeaponEnum.Pistol ? SubT[0] : SubT[1];
                break;
            case 3:
                if (ThrowingWeapon == null) return;
                target = ThrowingWeapon;
                targetTransform = ThrowingT;
                break;
            case 4:
                if (HealItem == null) return;
                target = HealItem;
                targetTransform = HealT;
                break;
        }
        _player.PlayerAnimCon.ChangeAnimationLayer(target.Data.WeaponType);
        _player.PlayerUI.UpdateAmmoAction?.Invoke(target.GetAmmoStatus());
        target.CanShot = true;
        DeactivateOtherWeapons(targetTransform);
    }

    void ChangeEquippedWeapon(ref Weapon equippedWeapon, Weapon newWeapon)
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.UnEquip();
        }
        equippedWeapon = newWeapon;
    }

    void DeactivateOtherWeapons(Transform activeTransform)
    {
        MainT.gameObject.SetActive(MainT == activeTransform);

        foreach (Transform sub in SubT)
        {
            sub.gameObject.SetActive(sub == activeTransform);
        }

        if (ThrowingT != null)
            ThrowingT.gameObject.SetActive(ThrowingT == activeTransform);

        if (HealT != null)
            HealT.gameObject.SetActive(HealT == activeTransform);
    }

    public void GetAmmo()
    {
        if (MainWeapon != null)
        {
            MainWeapon.GetAmmo();
        }
        SubWeapon.GetAmmo();

        if (MainT.gameObject.activeSelf)
        {
            _player.PlayerUI.UpdateAmmoAction.Invoke(MainWeapon.GetAmmoStatus());
        }
    }
}
