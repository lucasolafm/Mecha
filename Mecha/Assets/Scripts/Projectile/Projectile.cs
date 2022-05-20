using UnityEngine;

public class Projectile : Entity
{
    public override void OnHit()
    {
        gameObject.SetActive(false);
    }
}