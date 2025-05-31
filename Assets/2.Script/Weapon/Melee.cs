using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon, IShootable
{
    private MeleeData Data => base.Data as MeleeData;
    public virtual void Shot()
    {
        /*
        if (CanShot)
        {
            Debug.LogError($"{gameObject.name}의 canShot: false");
        }

        // Crosshair를 바라보게 방향 전환
        Vector3 lookDir = _playerInteract.InteractRay.direction;
        lookDir.y = 0f;
        lookDir = Quaternion.AngleAxis(30f, Vector3.up) * lookDir; // 30도 오른쪽으로 회전
        _character.forward = lookDir;

        // 공격 애니메이션 동작
        */
    }

    public override void GetAmmo() // 근접 무기는 총알 보급이 필요없음
    {
        return;
    }

    public override string GetAmmoStatus()
    {
        throw new System.NotImplementedException();
    }

    protected override void ConnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction += Shot;
    }

    protected override void DisconnectDelegate()
    {
        if (PlayerController == null) return;
        PlayerController.OnShotAction -= Shot;
    }
}
