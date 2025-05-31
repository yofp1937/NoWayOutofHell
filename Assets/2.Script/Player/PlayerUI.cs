using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("# Player UI Data")]
    [SerializeField] TextMeshProUGUI _interactText;
    [SerializeField] TextMeshProUGUI _ammoText;

    public void UpdateInteractText(string message)
    {
        _interactText.text = message;
    }

    public void UpdateAmmoText(string message)
    {
        _ammoText.text = message;
    }
}
