using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] private TextMeshProUGUI healthValueText;
    [SerializeField] private TextMeshProUGUI healthMaxText;
    
    private float _currentHealth;
    private int _maxHealth;

    void Awake()
    {
        Player.LeveledUp.AddListener(OnLeveledUp);
    }

    void Start()
    {
        ShiftMax(Player.Stats.MaxHealth);
        Shift(_maxHealth);
    }
    
    private void OnLeveledUp()
    {
        ShiftMax(Player.Stats.MaxHealthPerLevel);
        Shift(_maxHealth);
    }

    public void Shift(float value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
        
        healthBar.fillAmount = _currentHealth / _maxHealth;
        healthValueText.text = Mathf.Round(_currentHealth).ToString();
    }

    private void ShiftMax(int value)
    {
        _maxHealth += value;
        healthMaxText.text = "MAX " + _maxHealth;
    }
}