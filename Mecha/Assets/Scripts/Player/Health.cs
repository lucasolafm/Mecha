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

    [SerializeField] private float damagePlane;
    [SerializeField] private float damageMissile;

    private float _currentHealth;
    private int _maxHealth;
    
    void Awake()
    {
        Leveler.LeveledUp.AddListener(OnLeveledUp);
        Player.Attack.AddListener(OnAttack);
        Player.HitProjectile.AddListener(OnHitProjectile);
        
        _maxHealth = Player.Stats.MaxHealth;
        _currentHealth = _maxHealth;
    }

    private void OnLeveledUp()
    {
        ShiftMax(Player.Stats.MaxHealthPerLevel);
        _currentHealth = _maxHealth;
        ResetResourcesFromLevelUp.Invoke();
    }

    private void OnAttack(Enemy enemy, Vector2 position)
    {
        Shift(-damagePlane);
    }

    private void OnHitProjectile(Projectile projectile)
    {
        Shift(-damageMissile);
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
}