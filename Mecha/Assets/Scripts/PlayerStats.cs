
using System;

[Serializable]
public class PlayerStats : ICloneable
{
    public int MaxHealth;
    public float MoveSpeed;
    public float LaunchForce;
    public float BounceForce;
    public int MaxHealthPerLevel;
    
    public object Clone()
    {
        return MemberwiseClone();
    }
}