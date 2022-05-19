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
    public static UnityEvent<Enemy, Vector2> Attack = new UnityEvent<Enemy, Vector2>();
    public static UnityEvent FinishAttack = new UnityEvent();
    public static UnityEvent<Missile> HitMissile = new UnityEvent<Missile>();

    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerBaseStats baseStats;

    private Rigidbody2D _rb;
    private Health _health;
    private Experience _experience;
    private Leveler _leveler;
    private Mover _mover;
    private Comboer _comboer;
    private PhysicsHandler _physicsHandler;
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
        _mover = GetComponent<Mover>();
        _physicsHandler = GetComponent<PhysicsHandler>();
        _comboer = GetComponent<Comboer>();
        _leveler = GetComponent<Leveler>();
        _experience = GetComponent<Experience>();
        _health = GetComponent<Health>();

        GameManager.GameUnfreeze.AddListener(OnGameUnfreeze);

        Stats = (PlayerStats)baseStats.Stats.Clone();
        _prevPosition = transform.position;
        _prevPositionRb = _rb.position;
    }
    
    void Update()
    {
        InputManager.I.Tick();
        _mover.Tick();

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
            Attack.Invoke(_hitEnemy, _hitEnemy.transform.position);
        }

        if (collider.GetComponent<Entity>().GetType() == EntityType.Missile)
        {
            HitMissile.Invoke(collider.GetComponent<Missile>());
        }
    }

    private void OnGameUnfreeze()
    {
        FinishAttack.Invoke();
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
   
}
