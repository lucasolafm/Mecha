using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceDisplayer : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Experience experience;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarGain;
    [SerializeField] private Image healthBarLose;
    [SerializeField] private Image xpBar;
    [SerializeField] private Image xpBarGain;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private float animTime;
    [SerializeField] private float animDelay;

    private float _prevHealth;
    private float _prevXP;
    private float _shiftHealth;
    private float _shiftXP;
    private float _shiftHealthStart;
    private float _shiftXPStart;
    private float _currentShiftHealth;
    private float _currentShiftXP;
    private float _animElapsed;
    private float _animProgress;
    private Image _healthBarToAnimate;

    void Awake()
    {
        Health.ResetResourcesFromLevelUp.AddListener(OnResetResourcesFromLevelUp);
    }

    void Start()
    {
        healthBar.fillAmount = GetRatioHealth(health.GetCurrent());
        xpBar.fillAmount = xpBarGain.fillAmount = GetRatioXP(experience.GetCurrent());
    }
    
    void Update()
    {
        _currentShiftHealth = health.GetCurrent() - _prevHealth;
        _prevHealth = health.GetCurrent();
        _currentShiftXP = experience.GetCurrent() - _prevXP;
        _prevXP = experience.GetCurrent();

        if (_currentShiftHealth != 0 || _currentShiftXP != 0)
        {
            _shiftHealth += _currentShiftHealth;
            _shiftXP += _currentShiftXP;
            _shiftHealthStart = _shiftHealth;
            _shiftXPStart = _shiftXP;
            _animElapsed = 0;
            
            if (_currentShiftHealth < 0)
            {
                healthBar.fillAmount = GetRatioHealth(health.GetCurrent());
                healthBarLose.fillAmount = GetRatioHealth(health.GetCurrent() - _shiftHealthStart);
                healthBarGain.fillAmount = 0;
                _healthBarToAnimate = healthBarLose;
            }
            else if (_currentShiftHealth > 0)
            {
                healthBar.fillAmount = GetRatioHealth(health.GetCurrent() - _shiftHealthStart);
                healthBarGain.fillAmount = GetRatioHealth(health.GetCurrent());
                healthBarLose.fillAmount = 0;
                _healthBarToAnimate = healthBar;
            }

            if (_currentShiftXP > 0)
            {
                xpBar.fillAmount = GetRatioXP(experience.GetCurrent() - _shiftXPStart);
                xpBarGain.fillAmount = GetRatioXP(experience.GetCurrent());
            }
        }
        
        if (_shiftXP == 0 && _shiftHealth == 0 || GameManager.I.GameIsPaused) return;

        _animElapsed += Time.deltaTime;
        if (_animElapsed < animDelay) return;

        _animProgress = Anim.EaseOutSine(Mathf.Min((_animElapsed - animDelay) / animTime, 1));

        _shiftHealth = _shiftHealthStart * (1 - _animProgress);
        _shiftXP = _shiftXPStart * (1 - _animProgress);

        _healthBarToAnimate.fillAmount = GetRatioHealth(health.GetCurrent() - _shiftHealth);
        xpBar.fillAmount = GetRatioXP(experience.GetCurrent() - _shiftXP);
        
        healthText.text = Mathf.Round(health.GetCurrent() - _shiftHealth).ToString();
        xpText.text = Mathf.Round(experience.GetCurrent() - _shiftXP) + "%";
    }

    private void OnResetResourcesFromLevelUp()
    {
        healthBar.fillAmount = GetRatioHealth(health.GetCurrent());
        xpBar.fillAmount = xpBarGain.fillAmount = GetRatioXP(experience.GetCurrent());
        healthText.text = Mathf.Round(health.GetCurrent()).ToString();
        xpText.text = Mathf.Round(experience.GetCurrent()) + "%";
        _prevHealth = health.GetCurrent();
        _prevXP = experience.GetCurrent();
    }
    
    private float GetRatioHealth(float value)
    {
        return value / health.GetMax();
    }

    private float GetRatioXP(float value)
    {
        return value / experience.GetMax();
    }
}