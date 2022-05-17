using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Missile missilePrefab;
    public float moveSpeed;
    public float fireCooldownMin;
    public float fireCooldownMax;
    
    private bool _hidden;
    private float _currentLaunchCooldown;
    private int _direction;
    private bool _firingEnabled;
    private float _timeSinceFired;
    private Missile _missile;
    
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

    public void SetDirection(int direction)
    {
        _direction = direction;
    }

    public void SetFiringEnabled(bool value)
    {
        _firingEnabled = value;
    }

    public void SetHidden(bool value)
    {
        _hidden = value;
        GetRenderer().enabled = !_hidden;
        GetCollider().enabled = !_hidden;
    }

    private void Fire()
    {
        _missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        EntityManager.RegisterEntity(_missile);
    }

    private float GenerateLaunchCooldown()
    {
        return UnityEngine.Random.Range(fireCooldownMin, fireCooldownMax);
    }
}
