using TMPro;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boostsText;
    
    private Player _player;
    private bool _canBoost;
    private int _currentBoostAmount;

    void Awake()
    {
        _player = GetComponent<Player>();
        
        Player.HitFloor.AddListener(OnHitFloor);
        Player.Jumped.AddListener(OnJumped);
        Player.HitEntity.AddListener(OnHitEntity);
    }

    void Start()
    {
        ShiftBoostAmount(3);
    }

    private void OnHitFloor()
    {
        _canBoost = false;
    }
    
    private void OnJumped()
    {
        _canBoost = true;
    }
    
    private void OnHitEntity()
    {
        _canBoost = true;
    }

    public void OnHitHelperBotBoost()
    {
        _currentBoostAmount++;
    }
    
    public void Tick()
    {
        if (!InputManager.I.Tapped || !_canBoost ||  _currentBoostAmount == 0) return;
        
        Boost();
    }

    private void Boost()
    {
        return;
        _player.Launch(Player.Stats.BoostForce);
        ShiftBoostAmount(-1);
    }

    private void ShiftBoostAmount(int value)
    {
        _currentBoostAmount += value;
        boostsText.text = "Boosts: " + _currentBoostAmount;
    }
}