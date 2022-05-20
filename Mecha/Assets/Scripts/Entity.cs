using UnityEngine;

public enum EntityType
{
    Enemy,
    Missile,
    HelperBot
}

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityType type;

    [HideInInspector] public Transform Transform;
    [HideInInspector] public SpriteRenderer SpriteRenderer;
    private Collider2D _collider;
    private bool _isActive;

    void Awake()
    {
        Transform = transform;
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public virtual void OnHit() { }

    public Collider2D GetCollider()
    {
        return _collider;
    }
    
    public new EntityType GetType()
    {
        return type;
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public void SetIsActive(bool value)
    {
        _isActive = false;
    }
}