using UnityEngine;

public class Laser : Projectile
{
    [SerializeField] private float speed;
    
    void Update()
    {
        if (GameManager.I.GameIsPaused) return;
        
        transform.position += transform.right * speed * Time.deltaTime;
    }
}