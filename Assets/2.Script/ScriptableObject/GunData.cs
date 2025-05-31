using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AmmoData
{
    public int MaxAmmo; // 최대 총알 수
    public int MaxLoadedAmmo; // 장전 가능한 총알 수
    public int LoadedAmmo; // 장전된 총알 수
    public int RemainAmmo; // 남은 총알 수
    public float ReloadTime; // 장전에 필요한 시간
    public float BulletSpeed; // 총알의 속도

    public void Clone(GunData data)
    {
        MaxAmmo = data.MaxAmmo;
        MaxLoadedAmmo = data.MaxLoadedAmmo;
        LoadedAmmo = data.LoadedAmmo;
        RemainAmmo = data.RemainAmmo;
        ReloadTime = data.ReloadTime;
        BulletSpeed = data.BulletSpeed;
    }
}

[CreateAssetMenu(fileName = "NewGunData", menuName = "Weapons/GunData")]
public class GunData : WeaponData
{
    [Header("# Ammo Data")]
    public int MaxAmmo; // 최대 총알 수
    public int MaxLoadedAmmo; // 장전 가능한 총알 수
    public int LoadedAmmo; // 장전된 총알 수
    public int RemainAmmo; // 남은 총알 수
    public float ReloadTime; // 장전에 필요한 시간
    public float BulletSpeed; // 총알의 속도

    // TODO 반동(Recoil) 만들기
    // TODO 탄퍼짐(Spread) 만들기
    // TODO 정조준(Aiming) 만들기
}
