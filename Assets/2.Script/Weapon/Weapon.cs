using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Interactable
{
    [Header("# Weapon's Main Data")]
    public WeaponData Data;

    [Header("# Weapon's Self Component References Data")]
    protected Collider _col;
    protected Rigidbody _rigid;

    [Header("# Weapon's External References Data")]
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Transform Character;
    [HideInInspector] public PlayerController PlayerController;
    [HideInInspector] public PlayerItem PlayerItem;
    [HideInInspector] public PlayerInteract PlayerInteract;
    [HideInInspector] public PlayerUI PlayerUI;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _rigid = GetComponent<Rigidbody>();
        Init();
    }

    protected virtual void Init() { }

    void Start()
    {
        // 부모 오브젝트에 PlayerController가 있을 경우 자동으로 플레이어 참조를 초기화
        // (기본 장착중인 무기가 오류를 일으키지 않게 자동으로 Player를 초기화)
        PlayerController controller = GetComponentInParent<PlayerController>();
        if (controller != null)
        {
            Player = controller.gameObject;
            BindPlayerComponents();
            PlayerUI.UpdateAmmoText(GetAmmoStatus());
        }
    }

    // 무기를 교체할때 GameObject가 활성화됐다 비활성화됐다를 반복하므로 OnEnalbe과 OnDisable에서 Delegate를 갱신함
    void OnEnable()
    {
        ConnectDelegate();
    }
    void OnDisable()
    {
        DisconnectDelegate();
    }

    protected override void Interact(GameObject player)
    {
        Player = player;
        Equip();
    }

    public virtual void Equip()
    {
        BindPlayerComponents();
        PlayerItem.GetWeapon(this);
    }

    public virtual void UnEquip()
    {
        transform.parent = null;
        transform.position = Player.transform.position + Vector3.up;
        DisconnectDelegate();
        SetPosition();
        ResetPlayerBindings();
    }

    private void BindPlayerComponents()
    {
        // Debug.Log($"{gameObject.name}.BindPlayerComponents() 실행");
        if (Player == null) return;
        Character = Player.transform.Find("BananaMan");
        PlayerController = Player.GetComponent<PlayerController>();
        ConnectDelegate();
        PlayerItem = Player.GetComponent<PlayerItem>();
        PlayerInteract = Player.GetComponent<PlayerInteract>();
        PlayerUI = Player.GetComponent<PlayerUI>();
        _col.enabled = false;
        _rigid.useGravity = false;
    }

    private void ResetPlayerBindings()
    {
        Player = null;
        Character = null;
        PlayerController = null;
        PlayerItem = null;
        PlayerInteract = null;
        PlayerUI = null;
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
        transform.position = PlayerItem.MainT.position;
        transform.rotation = Quaternion.identity;
    }

    // TODO
    // 1.총알 획득
    // 3.총기 반동
    // 4.탄퍼짐
    // 5.정조준

    /// <summary>
    /// 총알 보급시 잔탄 가득 채워주는 함수
    // /// </summary>
    // public void GetBullet()
    // {
    //     if (0 > Data.MaxAmmo) return; // 총알이 무한인경우 return
    //     Data.RemainAmmo = Data.MaxAmmo - Data.LoadedAmmo;
    //     Debug.Log($"{gameObject.name} 총알 획득");
    // }

    public abstract void GetAmmo();
    public abstract string GetAmmoStatus();

    protected virtual void ConnectDelegate() { }
    protected virtual void DisconnectDelegate() { }
}
