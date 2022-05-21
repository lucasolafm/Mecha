using UnityEngine;

public class Projectile : Entity
{
    private int _tier;

    public int GetTier()
    {
        return _tier;
    }

    public void SetTier(int tier)
    {
        _tier = tier;
    }

    public override void OnHit()
    {
        gameObject.SetActive(false);
    }
}