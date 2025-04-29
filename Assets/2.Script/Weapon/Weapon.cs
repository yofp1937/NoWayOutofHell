using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponEnum {Rifle, ShotGun, Sniper, Pistol, Melee, ThrowingWeapon, HealItem }
public abstract class Weapon : Interactable
{
    [Header("# Weapon's Main Data")]
    public WeaponEnum WeaponEnum;
    public GameObject Bullet;
    protected Transform _muzzle; // 총구

    [Header("# Weapon's Reference Data")]
    [SerializeField] protected GameObject _player;
    [SerializeField] protected PlayerController _playerController;
    [SerializeField] protected PlayerItem _playerItem;
    Collider _col;
    Rigidbody _rigid;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _rigid = GetComponent<Rigidbody>();
    }

    protected override void Interact(GameObject player)
    {
        _player = player;
        Equip();
    }

    public virtual void Equip()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _playerItem = _player.GetComponent<PlayerItem>();
        
        if(_playerController != null)
        {
            _playerController.OnShotAction += Shot;
            _playerItem.GetWeapon(this);
            _col.enabled = false;
            _rigid.useGravity = false;
        }
        else
        {
            Debug.Log("PlayerController를 찾을 수 없음!");
        }
    }

    public virtual void UnEquip()
    {
        transform.parent = null;
        transform.position = _player.transform.position + Vector3.up;
        SetPosition();

        _playerController.OnShotAction -= Shot;
        _player = null;
        _playerController = null;
        _playerItem = null;
        _col.enabled = true;
        _rigid.useGravity = true;
    }

    /// <summary>
    /// 무기를 장착할때 사용하는 메서드
    /// </summary>
    public void SetPosition(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        Vector3 inverseScale = new Vector3(
            1f / parent.lossyScale.x,
            1f / parent.lossyScale.y,
            1f / parent.lossyScale.z
        );
        transform.localScale = inverseScale;
    }

    /// <summary>
    /// 다른 무기가 장착돼서 버려질때 사용하는 메서드
    /// </summary>
    public void SetPosition()
    {
        Vector3 scale = transform.localScale;
        transform.SetParent(GameManager.instance.InteractableT);
        transform.localPosition = _player.transform.position;
        transform.localRotation = Quaternion.identity;
        transform.localScale = scale;
    }

    public abstract void Shot();
}
