using System.Collections.Generic;
using UnityEngine;

public static class EntityManager
{
    private static Dictionary<Collider2D, Entity> _entityOfCollider = new Dictionary<Collider2D, Entity>();

    public static void RegisterEntity(Entity entity)
    {
        _entityOfCollider[entity.GetComponent<Collider2D>()] = entity;
    }

    public static Entity GetEntity(Collider2D collider)
    {
        return _entityOfCollider[collider];
    }
}