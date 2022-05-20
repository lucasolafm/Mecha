using UnityEngine;

public class ClusterShotEnemy : Enemy
{
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private Vector2 launchForce;

    private Missile _missile;
    
    protected override void Fire()
    {
        for (int i = 0; i < 4; i++)
        {
            _missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            _missile.RigidBody.AddForce(new Vector2(launchForce.x * (i < 2 ? -1 : 1) / (i % 2 == 0 ? 3 : 1), launchForce.y));
        }
    }
}