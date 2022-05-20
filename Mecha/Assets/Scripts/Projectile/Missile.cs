using System;
using UnityEngine;

public class Missile : Projectile
{
    [HideInInspector] public Rigidbody2D RigidBody;

    void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }
}