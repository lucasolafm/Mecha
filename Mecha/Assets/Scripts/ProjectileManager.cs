using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    void Awake()
    {
        Player.HitProjectile.AddListener(OnHitProjectile);
    }

    private void OnHitProjectile(Projectile projectile)
    {
        projectile.OnHit();
    }
}
