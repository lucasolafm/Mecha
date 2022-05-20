using UnityEngine;

public class QuadShotEnemy : Enemy
{
    [SerializeField] private Laser laserPrefab;

    protected override void Fire()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, 90 * i + 45));
        }
    }
}