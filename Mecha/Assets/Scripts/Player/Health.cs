using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] Image healthBarGain;
    [SerializeField] Image healthBarLose;
    [SerializeField] private TextMeshProUGUI healthValueText;
    [SerializeField] private TextMeshProUGUI healthMaxText;
    [SerializeField] private float barChangeAnimTime;
    [SerializeField] private float healthAnimDelay;
    [SerializeField] private float damagePlane;
    [SerializeField] private float damageMissile;

    private float _currentHealth;
    private int _maxHealth;
    private int _currentAnimDirection;
    private bool _changedDirection;
    private bool _animatingHealth;
    private float _animElapsed;
    private float _animStartValue;
    private Image _barToAnimate;

    void Awake()
    {
        Player.LeveledUp.AddListener(OnLeveledUp);
        GameManager.GamePause.AddListener(OnGamePause);
        GameManager.GameUnpause.AddListener(OnGameUnpause);
        Player.Attack.AddListener(OnAttack);
        Player.FinishAttack.AddListener(OnFinishAttack);
        Player.HitMissile.AddListener(OnHitMissile);
    }

    void Start()
    {
        ShiftMax(Player.Stats.MaxHealth);
        Shift(_maxHealth);
        healthBar.fillAmount = 1;
    }

    void Update()
    {
        if (!_animatingHealth) return;

        _animElapsed += Time.deltaTime;
        if (_animElapsed < healthAnimDelay) return;

        _barToAnimate.fillAmount = _animStartValue + (_currentHealth / _maxHealth - _animStartValue) * 
            Anim.EaseOutSine(Mathf.Min((_animElapsed - healthAnimDelay) / barChangeAnimTime, 1));

        if (_animElapsed < 1) return;

        _animatingHealth = false;
    }
    
    private void OnLeveledUp()
    {
        ShiftMax(Player.Stats.MaxHealthPerLevel);
        Shift(_maxHealth);
    }

    private void OnGamePause()
    {
        _animatingHealth = false;
    }

    private void OnGameUnpause()
    {
        _animatingHealth = true;
    }

    private void OnAttack(Enemy enemy, Vector2 position)
    {
        Shift(-damagePlane);
    }

    private void OnFinishAttack()
    {
        AnimateHealth();
    }

    private void OnHitMissile(Missile missile)
    {
        Shift(-damageMissile);
    }

    public void Shift(float value)
    {
        _changedDirection = _currentAnimDirection == 0 || (value < 0 ? -1 : 1) != _currentAnimDirection;
        _currentAnimDirection = value < 0 ? -1 : 1;

        healthBarLose.fillAmount = value < 0 && _changedDirection ? _currentHealth / _maxHealth : healthBarLose.fillAmount;

        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
        healthValueText.text = Mathf.Round(_currentHealth).ToString();

        if (value < 0)
        {
            healthBar.fillAmount = _currentHealth / _maxHealth;
            healthBarGain.fillAmount = 0;
        }
        else
        {
            healthBarGain.fillAmount = _currentHealth / _maxHealth;
            healthBarLose.fillAmount = 0;
        }
    }

    private void ShiftMax(int value)
    {
        _maxHealth += value;
        healthMaxText.text = "MAX " + _maxHealth;
    }

    private void AnimateHealth()
    {
        _animatingHealth = true;
        _animElapsed = 0;
        _barToAnimate = _currentAnimDirection < 0 ? healthBarLose : healthBar;
        _animStartValue = _barToAnimate.fillAmount;
    }
}