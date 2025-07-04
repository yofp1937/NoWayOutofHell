using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("# Main Data")]
    [HideInInspector] public Animator Anim;

    [Header("# Reference Data")]
    Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        Anim = _player.Character.GetComponent<Animator>();
    }

    /// <summary>
    /// 무기 획득시 호출하여 Player의 애니메이션 레이어 변경
    /// </summary>
    public void ChangeAnimationLayer(WeaponEnum weaponEnum)
    {
        string targetLayer = GetLayerNameByWeaponEnum(weaponEnum);

        for (int index = 0; index < Anim.layerCount; index++)
        {
            string layerName = Anim.GetLayerName(index);
            Anim.SetLayerWeight(index, layerName == targetLayer ? 1f : 0f);
        }
    }

    /// <summary>
    /// WeaponEnum을 애니메이션 레이어 이름으로 변환
    /// </summary>
    string GetLayerNameByWeaponEnum(WeaponEnum weaponEnum)
    {
        switch (weaponEnum)
        {
            case WeaponEnum.Rifle:
            case WeaponEnum.ShotGun:
                return "Main";
            case WeaponEnum.Pistol:
                return "Pistol";
            case WeaponEnum.Melee:
                return "Melee";
            case WeaponEnum.ThrowingWeapon:
                return "Throwing"; // Animator에 추가되면 자동 적용됨
            case WeaponEnum.HealItem:
                return "Heal";     // Animator에 추가되면 자동 적용됨
            default:
                Debug.LogWarning($"[PlayerAnimationController] 알 수 없는 무기 타입: {weaponEnum}");
                return "Pistol";
        }
    }

    public void SetMoveDirection(Vector3 localDir)
    {
        Anim.SetFloat("MoveX", localDir.x);
        Anim.SetFloat("MoveY", localDir.z);

        bool isMoving = localDir.sqrMagnitude > 0.01f;
        Anim.SetBool("IsMoving", isMoving);
    }
}
