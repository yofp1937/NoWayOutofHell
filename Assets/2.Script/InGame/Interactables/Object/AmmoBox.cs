using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    [Header("# Reference Data")]
    Collider _col;
    Rigidbody _rigid;

    protected override void Interact(GameObject player)
    {
        PlayerItem item = player.GetComponent<PlayerItem>();
        item.GetAmmo();
    }
}
