using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    void Awake()
    {
        Player.HitMissile.AddListener(OnHitMissile);
    }

    private void OnHitMissile(Missile missile)
    {
        missile.OnHit();
    }
}
