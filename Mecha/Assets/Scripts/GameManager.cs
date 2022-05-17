using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public static UnityEvent GamePause = new UnityEvent();
    public static UnityEvent GameUnpause = new UnityEvent();

    [SerializeField] private Player _player;
    
    [HideInInspector] public bool GameIsPaused;

    public float freezeTimeBase;
    public float freezeTimePerCombo;
    [SerializeField] private float freezeTimeLevelUp;

    private float _currentFreezeTime;
    private float _timeFrozen;
    private bool _unpauseNow;

    void Awake()
    {
        Player.HitEnemy.AddListener(OnHitEnemy);
        Player.LeveledUp.AddListener(OnLeveledUp);
        
        I = this;
    }

    void FixedUpdate()
    {
        if (!GameIsPaused) return;

        _timeFrozen += Time.fixedDeltaTime;
        if (_timeFrozen < _currentFreezeTime) return;
        _timeFrozen = 0;
        _currentFreezeTime = 0;

        UnpauseGame();
    }

    private void OnHitEnemy(Enemy enemy, Vector2 position)
    {
        FreezeGame(freezeTimeBase + freezeTimePerCombo * _player.GetCurrentCombo());
    }
    
    private void OnLeveledUp()
    {
        FreezeGame(freezeTimeLevelUp);
    }

    private void PauseGame()
    {
        GameIsPaused = true;
        GamePause.Invoke();
    }

    private void UnpauseGame()
    {
        GameIsPaused = false;
        GameUnpause.Invoke();
    }

    private void FreezeGame(float time)
    {
        PauseGame();
        _currentFreezeTime = Mathf.Max(time, _currentFreezeTime);
    }
}