using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Sprite standingSprite;
    [SerializeField] private Sprite risingSprite;
    [SerializeField] private Sprite risingTipSprite;
    [SerializeField] private Sprite fallingSprite;
    [SerializeField] private Sprite bouncingSprite;
    [SerializeField] private Sprite leveledUpSprite;
    [SerializeField] private Sprite[] attacksUpwards;
    [SerializeField] private Sprite[] attacksDownwards;
    [SerializeField] private Sprite[] attacksSideways;
    [SerializeField] private float jumpTipReachedHeight;

    private Player _player;
    private Transform _transform;
    private BoxCollider2D _collider;
    private bool _jumping;
    private bool _attacking;

    void Awake()
    {
        _player = GetComponent<Player>();
        _collider = GetComponent<BoxCollider2D>();
        
        Player.Landed.AddListener(OnLanded);
        Player.Jumped.AddListener(OnJumped);
        Player.Bounced.AddListener(OnBounced);
        Player.Attack.AddListener(OnAttack);
        Player.FinishAttack.AddListener(OnFinishAttack);
        Player.HitProjectile.AddListener(OnHitProjectile);
        Leveler.LeveledUp.AddListener(OnLeveledUp);
    }

    private void OnAttack(Enemy enemy)
    {
        _attacking = true;
        _jumping = true;
        renderer.flipX = transform.position.x - enemy.transform.position.x > 0;

        if (enemy.transform.position.y > _collider.bounds.center.y + _collider.bounds.size.y / 2)
        {
            renderer.sprite = attacksUpwards[Random.Range(0, attacksUpwards.Length)];
        }
        else if (enemy.transform.position.y < _collider.bounds.center.y - _collider.bounds.size.y / 2)
        {
            renderer.sprite = attacksDownwards[Random.Range(0, attacksDownwards.Length)];
        }
        else
        {
            renderer.sprite = attacksSideways[Random.Range(0, attacksSideways.Length)];
        }
    }

    private void OnFinishAttack()
    {
        _attacking = false;
    }

    private void OnHitProjectile(Projectile projectile)
    {
        _jumping = true;
    }

    private void OnBounced()
    {
        renderer.sprite = bouncingSprite;
        _jumping = false;
    }

    private void OnJumped()
    {
        _jumping = true;
    }

    private void OnLanded()
    {
        renderer.sprite = standingSprite;
        _jumping = false;
    }
    
    private void OnLeveledUp()
    {
        renderer.sprite = leveledUpSprite;
    }

    public void Tick()
    {
        if (_player.GetMovement().x != 0)
        {
            renderer.flipX = _player.GetMovement().x < 0;
        }
    }

    public void FixedTick()
    {
        if (!_jumping || _attacking) return;
        
        if (Mathf.Abs(_player.GetPhysicsMovement().y) < jumpTipReachedHeight)
        {
            renderer.sprite = risingTipSprite;
        }
        else
        {
            renderer.sprite = _player.GetPhysicsMovement().y > 0 ? risingSprite : fallingSprite;
        }
    }
}