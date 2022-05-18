using System;
using UnityEngine;

public class Missile : Entity
{
    public override void OnHit()
    {
        gameObject.SetActive(false);
    }
}