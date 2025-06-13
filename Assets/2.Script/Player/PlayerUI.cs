using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [Header("# Player UI Data")]
    [SerializeField] GameObject Crosshair;
    [SerializeField] TextMeshProUGUI _interactText;
    [SerializeField] TextMeshProUGUI _ammoText;

    [Header("# Action")]
    public Action<string> UpdateAmmoAction;

    void Awake()
    {
        UpdateAmmoAction = UpdateAmmoText;
    }

    public void UpdateInteractText(string message)
    {
        _interactText.text = message;
    }

    void UpdateAmmoText(string message)
    {
        _ammoText.text = message;
    }
}
