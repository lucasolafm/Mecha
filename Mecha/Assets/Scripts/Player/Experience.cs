using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Experience : MonoBehaviour
{
    public static UnityEvent ReachedMaxXP = new UnityEvent();

    public Image xpBar;
    public Image xpBarChange;
    [SerializeField] private TextMeshProUGUI currentXPText;
    public float maxXP;
    [SerializeField] private float xpGainBase;
    [SerializeField] private float xpGainPerCombo;
    [SerializeField] private float barChangeAnimTime;
    [SerializeField] private float xpAnimDelay;

    private Player _player;
    private float _currentXP;
    private bool _animatingXP;
    private float _animElapsed;
    private float _animStartValue;

    void Awake()
    {
        _player = GetComponent<Player>();

        Player.Attack.AddListener(OnAttack);
        Player.FinishAttack.AddListener(OnFinishAttack);
        GameManager.GamePause.AddListener(OnGamePause);
        GameManager.GameUnpause.AddListener(OnGameUnpause);
    }

    void Start()
    {
        xpBar.fillAmount = 0;
        currentXPText.text = 0 + "%";
    }

    void Update()
    {
        if (!_animatingXP) return;

        _animElapsed += Time.deltaTime;
        if (_animElapsed < xpAnimDelay) return;

        xpBar.fillAmount = _animStartValue + (_currentXP / maxXP - _animStartValue) *
            Anim.EaseOutSine(Mathf.Min((_animElapsed - xpAnimDelay) / barChangeAnimTime, 1));

        if (_animElapsed < 1) return;
        _animatingXP = false;
    }

    private void OnAttack(Enemy enemy, Vector2 position)
    {
        Shift(xpGainBase + xpGainPerCombo * _player.GetCurrentCombo());
    }

    private void OnFinishAttack()
    {
        AnimateXP();
    }

    private void OnGamePause()
    {
        _animatingXP = false;
    }

    private void OnGameUnpause()
    {
        _animatingXP = true;
    }

    public void Shift(float value)
    {
        _currentXP += value;
        xpBarChange.fillAmount = _currentXP / maxXP;
        currentXPText.text = Mathf.Round(_currentXP) + "%";

        if (_currentXP < maxXP) return;

        Shift(-maxXP);
        ReachedMaxXP.Invoke();
    }
    
    private void AnimateXP()
    {
        _animatingXP = true;
        _animElapsed = 0;
        _animStartValue = xpBar.fillAmount;
    }
}