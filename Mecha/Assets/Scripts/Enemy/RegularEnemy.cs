using UnityEngine;

public class RegularEnemy : Enemy
{
    [SerializeField] private Missile missilePrefab;
    
    private Missile _missile;
    
    protected override void Fire()
    {
        _missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
    }
}