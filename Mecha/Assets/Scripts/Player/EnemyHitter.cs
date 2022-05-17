using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitter : MonoBehaviour
{
    public Player player;


    void Awake()
    {
        Player.HitEnemy.AddListener(OnHitEntity);
    }

    private void OnHitEntity(Entity entity, Vector2 position)
    {
        if (entity.GetType() == EntityType.Missile)
        {
            
        }
        else if (entity.GetType() == EntityType.Enemy)
        {

        }
        
        
    }
}