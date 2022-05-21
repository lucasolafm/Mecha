using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static UnityEvent ResetResourcesFromLevelUp = new UnityEvent();
    
    [SerializeField] private TextMeshProUGUI healthMaxText;
    
    private float _currentHealth;
    private int _maxHealth;
    private DifficultySettings _difficultySettings;
    
    void Awake()
    {
        Leveler.LeveledUp.AddListener(OnLeveledUp);
        Player.Attack.AddListener(OnAttack);
        Player.HitProjectile.AddListener(OnHitProjectile);
        
        _maxHealth = GameManager.I.GetDifficultySettings().MaxHealthBase;
        _currentHealth = _maxHealth;
        _difficultySettings = GameManager.I.GetDifficultySettings();
    }

    private void OnLeveledUp()
    {
        ShiftMax(GameManager.I.GetDifficultySettings().MaxHealthPerLevel);
        _currentHealth = _maxHealth;
        ResetResourcesFromLevelUp.Invoke();
    }

    private void OnAttack(Enemy enemy)
    {
        Shift(-GetEnemyDamage(enemy.GetTier()));
    }

    private void OnHitProjectile(Projectile projectile)
    {
        Shift(-GetEnemyDamage(projectile.GetTier()) * _difficultySettings.RatioDamageMissile);
    }

    public float GetCurrent()
    {
        return _currentHealth;
    }

    public int GetMax()
    {
        return _maxHealth;
    }

    private void Shift(float value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
    }

    private void ShiftMax(int value)
    {
        _maxHealth += value;
        healthMaxText.text = "MAX " + _maxHealth;
    }

    private float GetEnemyDamage(int tier)
    {
        return _difficultySettings.DamageBaseEnemy +
               _difficultySettings.DamageBonusEnemyCurve((float) tier / (_difficultySettings.AmountTiers - 1)) *
               _difficultySettings.MaxDamageBonusEnemy;
    }
}