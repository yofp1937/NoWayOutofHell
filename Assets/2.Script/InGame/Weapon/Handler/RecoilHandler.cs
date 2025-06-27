using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
    [SerializeField] Transform _player;

    public void Recoil(Vector3 recoilKickBack, float recoilAmount)
    {
        Vector3 recoilVector = new Vector3(Random.Range(-recoilKickBack.x, recoilKickBack.x), recoilKickBack.y, recoilKickBack.z);
        Vector3 recoilCamVector = new Vector3(-recoilVector.y * 400f, recoilVector.x * 200f, 0);

        _player.localPosition = Vector3.Lerp(_player.localPosition, _player.localPosition + recoilVector, recoilAmount / 2f);// position recoil
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles + recoilCamVector), recoilAmount); // cam Recoil
    }

    public void RecoilBack()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 2f);
    }
}
