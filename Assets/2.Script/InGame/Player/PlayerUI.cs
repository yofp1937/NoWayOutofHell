using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("# Player UI Data")]
    TextMeshProUGUI _interactText;
    TextMeshProUGUI _ammoText;

    [Header("# Health Bar")]
    Slider _hpSlider;
    TextMeshProUGUI _hpText;

    [Header("# Damage Overlay")]
    [SerializeField] float _duration = 0.3f; // overlay 지속시간
    [SerializeField] float _fadeSpeed = 0.5f; // overlay 사라지는 시간
    float _overlayTimer;
    Image _damageOverlay;

    [Header("# Action")]
    public Action<string> UpdateAmmoAction;

    void Awake()
    {
        UpdateAmmoAction = UpdateAmmoText;
    }

    void Update()
    {
        UpdateDamageOverlay();
    }

    public void ConnectComponents(PlayerUICanvas playerUICanvas)
    {
        _interactText = playerUICanvas.InteractText;
        _ammoText = playerUICanvas.AmmoText;
        _hpSlider = playerUICanvas.HpSlider;
        _hpText = playerUICanvas.HpText;
        _damageOverlay = playerUICanvas.DamageOverlay;
    }

    public void UpdateInteractText(string message)
    {
        _interactText.text = message;
    }

    void UpdateAmmoText(string message)
    {
        _ammoText.text = message;
    }

    /// <summary>
    /// 플레이어 체력바 업데이트
    /// </summary>
    public void UpdateHealthUI(float maxHp, float currentHp)
    {
        _hpSlider.value = currentHp / maxHp;
        _hpText.text = currentHp.ToString();
    }

    /// <summary>
    /// 피해 입을시 화면 반짝이게 하는 효과
    /// </summary>
    void UpdateDamageOverlay()
    {
        if (_damageOverlay.color.a > 0) // color.a는 투명도
        {
            _overlayTimer += Time.deltaTime;
            if (_overlayTimer >= _duration)
            {
                // overlay 점차 사라지게
                float tempA = _damageOverlay.color.a;
                tempA -= Time.deltaTime * _fadeSpeed;
                _damageOverlay.color = new Color(_damageOverlay.color.r, _damageOverlay.color.g, _damageOverlay.color.b, tempA);
            }
        }
    }

    public void ShowDamageOverlay()
    {
        _overlayTimer = 0;
        _damageOverlay.color = new Color(_damageOverlay.color.r, _damageOverlay.color.g, _damageOverlay.color.b, 0.25f);
    }
}
