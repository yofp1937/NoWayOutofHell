using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Interactable
{
    [Header("# Weapon's Main Data")]
    public WeaponData Data;
    public bool CanShot;

    [Header("# Weapon's Self Component References Data")]
    [HideInInspector] public Collider Collider;
    protected Rigidbody _rigid;

    [Header("# Weapon's External References Data")]
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Transform ObjectArm;
    [HideInInspector] public Transform Character;
    [HideInInspector] public PlayerController PlayerController;
    [HideInInspector] public PlayerItem PlayerItem;
    [HideInInspector] public PlayerInteract PlayerInteract;
    [HideInInspector] public PlayerUI PlayerUI;
    [HideInInspector] public PlayerLook PlayerLook;

    void Awake()
    {
        Collider = GetComponent<Collider>();
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
            PlayerUI.UpdateAmmoAction?.Invoke(GetAmmoStatus());
            _rigid.constraints = RigidbodyConstraints.FreezeAll;
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
        _rigid.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void UnEquip()
    {
        transform.parent = null;
        transform.position = Player.transform.position + Vector3.up;
        DisconnectDelegate();
        SetPosition();
        ResetPlayerBindings();
        _rigid.constraints = RigidbodyConstraints.None;
    }

    private void BindPlayerComponents()
    {
        // Debug.Log($"{gameObject.name}.BindPlayerComponents() 실행");
        if (Player == null) return;
        Character = Player.GetComponentInChildren<Animator>().gameObject.transform;
        PlayerController = Player.GetComponent<PlayerController>();
        PlayerItem = Player.GetComponent<PlayerItem>();
        PlayerInteract = Player.GetComponent<PlayerInteract>();
        PlayerUI = Player.GetComponent<PlayerUI>();
        PlayerLook = Player.GetComponent<PlayerLook>();
        gameObject.layer = LayerMask.NameToLayer("Player");
        ConnectDelegate();
        _rigid.useGravity = false;
    }

    private void ResetPlayerBindings()
    {
        Player = null;
        ObjectArm = null;
        Character = null;
        PlayerController = null;
        PlayerItem = null;
        PlayerInteract = null;
        PlayerUI = null;
        PlayerLook = null;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
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

    public abstract void GetAmmo();
    public abstract string GetAmmoStatus();

    protected virtual void ConnectDelegate() { }
    protected virtual void DisconnectDelegate() { }
}
