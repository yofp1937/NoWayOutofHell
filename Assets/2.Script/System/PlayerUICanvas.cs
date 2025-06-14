using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvas : MonoBehaviour
{
    [Header("# PlayerUI Components")]
    public TextMeshProUGUI InteractText;
    public TextMeshProUGUI AmmoText;
    public Slider HpSlider;
    public TextMeshProUGUI HpText;
    public Image DamageOverlay;
}
