using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Experience : MonoBehaviour
{
    public static UnityEvent ReachedMaxXP = new UnityEvent();
    
    public float maxXP;
    [SerializeField] private float xpGainBase;
    [SerializeField] private float xpGainPerCombo;

    private Player _player;
    private float _currentXP;
    
    void Awake()
    {
        _player = GetComponent<Player>();

        Player.Attack.AddListener(OnAttack);
    }
    
    private void OnAttack(Enemy enemy)
    {
        Shift(xpGainBase + xpGainPerCombo * _player.GetCurrentCombo());
    }

    public float GetCurrent()
    {
        return _currentXP;
    }

    public float GetMax()
    {
        return maxXP;
    }

    private void Shift(float value)
    {
        _currentXP += value;

        if (_currentXP < maxXP) return;

        Shift(-maxXP);
        ReachedMaxXP.Invoke();
    }
}