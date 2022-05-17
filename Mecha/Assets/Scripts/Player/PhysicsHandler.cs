using System;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public float fallingGravityMultiplier;
    
    private float _launchForce;
    private bool _launchAfterResume;

    void Awake()
    {
        GameManager.GameUnpause.AddListener(OnGameResume);
        Player.HitEnemy.AddListener(OnHitEnemy);
        Player.HitMissile.AddListener(OnHitMissile);
    }

    private void OnGameResume()
    {
        if (!_launchAfterResume) return;
        _launchAfterResume = false;

        rb.velocity = Vector2.zero;
        Launch(Player.Stats.LaunchForce);
    }
    
    private void OnHitEnemy(Enemy enemy, Vector2 position)
    {
        _launchAfterResume = true;
    }

    private void OnHitMissile()
    {
        if (GameManager.I.GameIsPaused) return;
        
        Launch(Player.Stats.LaunchForce);
    }

    public void FixedTick()
    {
        rb.gravityScale = rb.velocity.y > 0 ? 1 : 1 + fallingGravityMultiplier;
        
        if (_launchForce == 0) return;

        AddForce(_launchForce);
        _launchForce = 0;
    }
    
    public void Launch(float force)
    {
        _launchForce = force;
    }

    private void AddForce(float force)
    {
        rb.velocity = new Vector2(0, force / rb.mass * Time.fixedDeltaTime);
    }
}