using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _anim;

    /// <summary>
    /// 무기 획득시 호출하여 Player의 애니메이션 레이어 변경
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeAnimationLayer(Weapon weapon)
    {
        string targetLayer = weapon.WeaponEnum.ToString();

        for(int index = 0; index < _anim.layerCount; index++)
        {
            string layerName = _anim.GetLayerName(index);

            if(layerName == targetLayer)
            {
                _anim.SetLayerWeight(index, 1f);
            }
            else
            {
                _anim.SetLayerWeight(index, 0f);
            }
        }
    }

    public void SetMove(bool move) { _anim.SetBool("Move", move); }
    public void Shot() { _anim.SetTrigger("Shot"); Debug.Log("Shot!"); }
}
