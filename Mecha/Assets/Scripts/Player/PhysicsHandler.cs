using System;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public float fallingGravityMultiplier;
    
    private float _launchForce;

    void Awake()
    {
        Player.FinishAttack.AddListener(OnFinishAttack);
        Player.HitProjectile.AddListener(OnHitProjectile);
        Player.Bounced.AddListener(OnBounced);
    }

    private void OnFinishAttack()
    {
        rb.velocity = Vector2.zero;
        Launch(Player.Stats.LaunchForce);
    }

    private void OnHitProjectile(Projectile projectile)
    {
        if (GameManager.I.GameIsPaused) return;
        
        Launch(Player.Stats.LaunchForce);
    }

    private void OnBounced()
    {
        Launch(Player.Stats.BounceForce);
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