using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public static UnityEvent GamePause = new UnityEvent();
    public static UnityEvent GameUnpause = new UnityEvent();
    public static UnityEvent GameUnfreeze = new UnityEvent();

    [SerializeField] private Player _player;
    
    [HideInInspector] public bool GameIsPaused;

    public float freezeTimeBase;
    public float freezeTimePerCombo;
    [SerializeField] private float freezeTimeLevelUp;

    private bool _freezing;
    private float _currentFreezeTime;
    private float _timeFrozen;
    private bool _unpauseNow;

    void Awake()
    {
        Player.Attack.AddListener(OnAttack);
        Leveler.LeveledUp.AddListener(OnLeveledUp);
        
        I = this;
    }

    void FixedUpdate()
    {
        if (!_freezing) return;

        _timeFrozen += Time.fixedDeltaTime;
        if (_timeFrozen < _currentFreezeTime) return;
        _timeFrozen = 0;
        _currentFreezeTime = 0;

        _freezing = false;
        GameUnfreeze.Invoke();
        UnpauseGame();
    }

    private void OnAttack(Enemy enemy, Vector2 position)
    {
        FreezeGame(freezeTimeBase + freezeTimePerCombo * _player.GetCurrentCombo());
    }
    
    private void OnLeveledUp()
    {
        FreezeGame(freezeTimeLevelUp);
    }

    public void PauseGame()
    {
        GameIsPaused = true;
        GamePause.Invoke();
    }

    public void UnpauseGame()
    {
        GameIsPaused = false;
        GameUnpause.Invoke();
    }

    private void FreezeGame(float time)
    {
        _freezing = true;
        PauseGame();
        _currentFreezeTime = Mathf.Max(time, _currentFreezeTime);
    }
}