using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static PlayerStats Stats;
    public static UnityEvent Jumped = new UnityEvent();
    public static UnityEvent HitFloor = new UnityEvent();
    public static UnityEvent Landed = new UnityEvent();
    public static UnityEvent Bounced = new UnityEvent();
    public static UnityEvent HitEntity = new UnityEvent();
    public static UnityEvent<Enemy, Vector2> HitEnemy = new UnityEvent<Enemy, Vector2>();
    public static UnityEvent HitMissile = new UnityEvent();
    public static UnityEvent LeveledUp = new UnityEvent();

    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerBaseStats baseStats;
    public float damagePlane;
    public float damageMissile;
    public float xpGainBase;
    public float xpGainPerCombo;

    private Rigidbody2D _rb;
    private Health _health;
    private Experience _experience;
    private Leveler _leveler;
    private Mover _mover;
    private Jumper _jumper;
    private Comboer _comboer;
    private PhysicsHandler _physicsHandler;
    private Booster _booster;
    private Bouncer _bouncer;
    private PlayerAnimator _animator;
    private Enemy _hitEnemy;
    private Vector3 _prevPosition;
    private Vector2 _prevPositionRb;
    private Vector2 _currentMovement;
    private Vector2 _currentMovementRb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<PlayerAnimator>();
        _bouncer = GetComponent<Bouncer>();
        _booster = GetComponent<Booster>();
        _jumper = GetComponent<Jumper>();
        _mover = GetComponent<Mover>();
        _physicsHandler = GetComponent<PhysicsHandler>();
        _comboer = GetComponent<Comboer>();
        _leveler = GetComponent<Leveler>();
        _experience = GetComponent<Experience>();
        _health = GetComponent<Health>();

        Stats = (PlayerStats)baseStats.Stats.Clone();
        _prevPosition = transform.position;
        _prevPositionRb = _rb.position;
    }
    
    void Update()
    {
        InputManager.I.Tick();
        _mover.Tick();
        _booster.Tick();
        _jumper.Tick();

        _currentMovement = transform.position - _prevPosition;
        _prevPosition = transform.position;
        
        _animator.Tick();
        cameraController.Tick();
    }

    void FixedUpdate()
    {
        _physicsHandler.FixedTick();
        
        _currentMovementRb = _rb.position - _prevPositionRb;
        _prevPositionRb = _rb.position;
        
        _animator.FixedTick();
        cameraController.FixedTick();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor")) return;
        
        HitFloor.Invoke();

        if (_bouncer.CanBounce())
        {
            _bouncer.Bounce();
            Bounced.Invoke();
        }
        else
        {
            Landed.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Entity")) return;
        
        HitEntity.Invoke();
        
        _hitEnemy = EnemyManager.GetEnemy(collider);
        if (_hitEnemy)
        {
            HitEnemy.Invoke(_hitEnemy, _hitEnemy.transform.position);
            OnHitEnemy(_hitEnemy);
        }

        if (collider.GetComponent<Entity>().GetType() == EntityType.Missile)
        {
            OnHitMissile(collider.gameObject);
            HitMissile.Invoke();
        }
    }

    private void OnHitEnemy(Enemy enemy)
    {
        ShiftHealth(-damagePlane);
        IncreaseCombo();
        ShiftXP(xpGainBase + xpGainPerCombo * GetCurrentCombo());
    }

    private void OnHitMissile(GameObject missile)
    {
        ShiftHealth(-damageMissile);
        missile.SetActive(false);
    }

    public Vector2 GetMovement()
    {
        return _currentMovement;
    }
    
    public Vector2 GetPhysicsMovement()
    {
        return _currentMovementRb;
    }

    public int GetCurrentCombo()
    {
        return _comboer.GetCurrentCombo();
    }

    public void ShiftHealth(float value)
    {
        _health.Shift(value);
    }
    
    public void ShiftXP(float value)
    {
        _experience.Shift(value);

        if (!_experience.HasReachedMax()) return;
        
        LevelUp();
    }

    public void ResetXP()
    {
        _experience.Reset();
    }
    
    public void IncreaseCombo()
    {
        _comboer.IncreaseCombo();
    }

    private void LevelUp()
    {
        _leveler.LevelUp();
        LeveledUp.Invoke();
    }
    
    public void Launch(float force)
    {
        _physicsHandler.Launch(force);
    }
}
