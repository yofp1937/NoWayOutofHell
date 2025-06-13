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
    public float AmmoSpeed; // 총알의 속도
    public Vector3 RecoilKickBack;
    public float RecoilAmount;

    public void Clone(GunData data)
    {
        MaxAmmo = data.MaxAmmo;
        MaxLoadedAmmo = data.MaxLoadedAmmo;
        LoadedAmmo = data.LoadedAmmo;
        RemainAmmo = data.RemainAmmo;
        ReloadTime = data.ReloadTime;
        AmmoSpeed = data.AmmoSpeed;
        RecoilKickBack = data.RecoilKickBack;
        RecoilAmount = data.RecoilAmount;
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
    public float AmmoSpeed; // 총알의 속도

    [Header("# Recoil Data")]
    public Vector3 RecoilKickBack = new Vector3(0.05f, 0.1f, 0f); // x: 좌우 반동, y: 수직 반동, z: 반동
    public float RecoilAmount = 0.1f; // 값이 클수록 반동이 커짐

    [Header("# Audio Data")]
    public AudioClip ShotAudio;
    public AudioClip ReloadAudio;

    // TODO 정조준(Aiming) 만들기
}
