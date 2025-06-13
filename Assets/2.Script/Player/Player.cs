using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerController PlayerCon;
    [HideInInspector] public PlayerHealth PlayerHealth;
    [HideInInspector] public PlayerInteract PlayerInteract;
    [HideInInspector] public PlayerItem PlayerItem;
    [HideInInspector] public PlayerLook PlayerLook;
    [HideInInspector] public PlayerMovement PlayerMovement;
    [HideInInspector] public PlayerAnimationController PlayerAnimCon;
    [HideInInspector] public PlayerUI PlayerUI;
    [HideInInspector] public GameObject Character;

    void Awake()
    {
        PlayerCon = GetComponent<PlayerController>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerInteract = GetComponent<PlayerInteract>();
        PlayerItem = GetComponent<PlayerItem>();
        PlayerLook = GetComponent<PlayerLook>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimCon = GetComponent<PlayerAnimationController>();
        PlayerUI = GetComponent<PlayerUI>();
        Character = transform.Find("BananaMan").gameObject;
    }
}
