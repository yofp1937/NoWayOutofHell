using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("# Main Data")]
    [SerializeField] float _health;
    float _maxHealth = 100f;
    
    [Header("# Health Bar")]
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _healthText;

    [Header("# Damage Overlay")]
    [SerializeField] Image _overlay;
    [SerializeField] float _duration; // overlay 지속시간
    [SerializeField] float _fadeSpeed; // overlay 사라지는 시간
    float _overlayTimer;


    void Start()
    {
        _health = _maxHealth;
        UpdateHealthUI();
        UpdateDamageOverlay();
    }

    void Update()
    {
        _health = Math.Clamp(_health, 0, _maxHealth); // 체력이 0 ~ MaxHealth만큼 유지되게 설정
        UpdateHealthUI();
        UpdateDamageOverlay();
    }

    /// <summary>
    /// 플레이어 체력바 업데이트
    /// </summary>
    void UpdateHealthUI()
    {
        _slider.value = _health / _maxHealth;
        _healthText.text = _health.ToString();
    }

    /// <summary>
    /// 피해 입을시 화면 반짝이게 하는 효과
    /// </summary>
    void UpdateDamageOverlay()
    {
        if(_overlay.color.a > 0) // color.a는 투명도
        {
            _overlayTimer += Time.deltaTime;
            if(_overlayTimer >= _duration)
            {
                // overlay 점차 사라지게
                float tempA = _overlay.color.a;
                tempA -= Time.deltaTime * _fadeSpeed;
                _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, tempA);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _overlayTimer = 0;
        _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, 0.25f);
    }

    public void RestroeHealth(float healAmount)
    {
        _health += healAmount;
    }
}
