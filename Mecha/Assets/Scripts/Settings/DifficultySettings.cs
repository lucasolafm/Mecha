using UnityEngine;

[CreateAssetMenu]
public class DifficultySettings : ScriptableObject
{
    public int MaxHealthBase;
    public int MaxHealthPerLevel;
    public int AmountExistEnemy;
    public int AmountExistPerEnemySpecial;
    public int AmountTiers;
    public float TimeToMaxTier;
    public float DamageBaseEnemy;
    public float MaxDamageBonusEnemy;
    public float DamageBonusEnemyCurve(float x) { return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)); }
    public float RatioDamageMissile;
}