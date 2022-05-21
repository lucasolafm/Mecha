using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private Sprite[] tierSprites;
    public float moveSpeed;
    public float fireCooldownMin;
    public float fireCooldownMax;

    private bool _hidden;
    private int _tier;
    private float _currentLaunchCooldown;
    private int _direction;
    private bool _firingEnabled;
    private float _timeSinceFired;

    void Start()
    {
        _currentLaunchCooldown = GenerateLaunchCooldown();
    }

    void Update()
    {
        if (GameManager.I.GameIsPaused) return;
        
        transform.position += new Vector3(moveSpeed * Time.deltaTime * _direction, 0);
        
        _timeSinceFired += Time.deltaTime;
        if (_timeSinceFired < _currentLaunchCooldown) return;
        _timeSinceFired = 0;
        _currentLaunchCooldown = GenerateLaunchCooldown();

        if (!_firingEnabled || _hidden) return;
        
        Fire();
    }

    public override void OnHit()
    {
        gameObject.SetActive(false);
    }

    public int GetTier()
    {
        return _tier;
    }

    public void SetTier(int tier)
    {
        _tier = tier;
        SpriteRenderer.sprite = tierSprites[Mathf.Min(_tier, tierSprites.Length - 1)];
    }

    public void SetDirection(int direction)
    {
        _direction = direction;
        SpriteRenderer.flipX = _direction < 0;
    }

    public void SetFiringEnabled(bool value)
    {
        _firingEnabled = value;
    }

    public void SetHidden(bool value)
    {
        _hidden = value;
        SpriteRenderer.enabled = !_hidden;
        GetCollider().enabled = !_hidden;
    }

    protected virtual void Fire() { }

    private float GenerateLaunchCooldown()
    {
        return UnityEngine.Random.Range(fireCooldownMin, fireCooldownMax);
    }
}
