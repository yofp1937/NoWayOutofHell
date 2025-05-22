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
    protected GameObject _player;
    protected Transform _character;
    protected PlayerController _playerController;
    protected PlayerItem _playerItem;
    protected PlayerInteract _playerInteract;
    Collider _col;
    Rigidbody _rigid;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _rigid = GetComponent<Rigidbody>();

        // 부모 오브젝트에 PlayerController가 있을 경우 자동으로 플레이어 참조를 초기화
        // (기본 장착중인 무기가 오류를 일으키지 않게 자동으로 Player를 초기화)
        PlayerController controller = GetComponentInParent<PlayerController>();
        if (controller != null)
        {
            _player = controller.gameObject;
            _character = _player.transform.Find("BananaMan");
            _playerController = _player.GetComponent<PlayerController>();
            ConnectShotDelegate();
            _playerItem = _player.GetComponent<PlayerItem>();
            _playerInteract = _player.GetComponent<PlayerInteract>();
            _col.enabled = false;
            _rigid.useGravity = false;
        }
    }

    void OnEnable()
    {
        ConnectShotDelegate();
    }

    void OnDisable()
    {
        DisconnectShotDelegate();
    }

    protected override void Interact(GameObject player)
    {
        _player = player;
        Equip();
    }

    public virtual void Equip()
    {
        _character = _player.transform.Find("BananaMan");
        _playerController = _player.GetComponent<PlayerController>();
        _playerItem = _player.GetComponent<PlayerItem>();
        _playerInteract = _player.GetComponent<PlayerInteract>();

        ConnectShotDelegate();
        _playerItem.GetWeapon(this);
        _col.enabled = false;
        _rigid.useGravity = false;
    }

    public virtual void UnEquip()
    {
        transform.parent = null;
        transform.position = _player.transform.position + Vector3.up;
        SetPosition();

        DisconnectShotDelegate();
        _player = null;
        _character = null;
        _playerController = null;
        _playerItem = null;
        _playerInteract = null;
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
        _rigid.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// 다른 무기가 장착돼서 버려질때 사용하는 메서드
    /// </summary>
    public void SetPosition()
    {
        transform.SetParent(GameManager.Instance.InteractableT);
        transform.position = _playerItem.MainT.position;
        transform.rotation = Quaternion.identity;
    }

    public virtual void Shot()
    {
        Debug.Log($"{gameObject.name}'s Shot!");

        // Crosshair를 바라보게 방향 전환
        Vector3 lookDir = _playerInteract.InteractRay.direction;
        lookDir.y = 0f;
        lookDir = Quaternion.AngleAxis(30f, Vector3.up) * lookDir; // 30도 오른쪽으로 회전
        _character.forward = lookDir;

        // 이후 총알이 있는 객체들은 총알 발사 로직을 작성
        // 총알이 없는 객체들은 공격 애니메이션 동작
        // 자식 스크립트에서 오버라이드해서 작성할것
    }


    void ConnectShotDelegate()
    {
        if (_playerController == null) return;
        _playerController.OnShotAction -= Shot;
        _playerController.OnShotAction += Shot;
    }

    void DisconnectShotDelegate()
    {
        if (_playerController == null) return;
        _playerController.OnShotAction -= Shot;
    }
}
