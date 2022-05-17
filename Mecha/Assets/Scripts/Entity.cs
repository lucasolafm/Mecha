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
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private bool _isActive;

    void Awake()
    {
        Transform = transform;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public SpriteRenderer GetRenderer()
    {
        return _renderer;
    }
    
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