using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponEnum { Pistol, Rifle, Melee }
public class Weapon : MonoBehaviour
{
    public WeaponEnum WeaponEnum;

    [Header("# Reference Data")]
    PlayerAnimationController _playerAnimationController;

    void Awake()
    {
        _playerAnimationController = GameObject.Find("Player").GetComponent<PlayerAnimationController>();
    }

    void OnEnable()
    {
        _playerAnimationController.ChangeAnimationLayer(this);
    }
}
