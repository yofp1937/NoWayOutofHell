using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponEnum { Rifle, ShotGun, Pistol, Melee, ThrowingWeapon, HealItem }

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("# Main Data")]
    public WeaponEnum WeaponType;

    [Header("# Shot Data")]
    public int Damage;
    public float FireRate; // 발사 주기
}