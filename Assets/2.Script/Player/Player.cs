using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Player Reference Data")]
    [HideInInspector] public PlayerController PlayerCon;
    [HideInInspector] public Hp PlayerHp;
    [HideInInspector] public PlayerInteract PlayerInteract;
    [HideInInspector] public PlayerItem PlayerItem;
    [HideInInspector] public PlayerLook PlayerLook;
    [HideInInspector] public PlayerMovement PlayerMovement;
    [HideInInspector] public PlayerAnimationController PlayerAnimCon;
    [HideInInspector] public PlayerUI PlayerUI;
    [HideInInspector] public GameObject Character;

    [Header("# Sub Reference Data")]
    public Transform ObjectArm;
    public Camera Camera;

    void Awake()
    {
        PlayerCon = GetComponent<PlayerController>();
        PlayerHp = GetComponent<Hp>();
        PlayerInteract = GetComponent<PlayerInteract>();
        PlayerItem = GetComponent<PlayerItem>();
        PlayerLook = GetComponent<PlayerLook>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimCon = GetComponent<PlayerAnimationController>();
        PlayerUI = GetComponent<PlayerUI>();
        Character = transform.Find("BananaMan").gameObject;

        ObjectArm = transform.Find("ObjectArm");
        Camera = ObjectArm.GetComponentInChildren<Camera>();
        
        BindPlayerHpEvents();
    }

    void BindPlayerHpEvents()
    {
        PlayerHp.OnDamageTaken += () =>
        {
            PlayerUI.UpdateHealthUI(PlayerHp.MaxHp, PlayerHp.CurrentHp);
            PlayerUI.ShowDamageOverlay();
        };
    }
}
