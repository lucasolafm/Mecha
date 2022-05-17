using System.Collections;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public Player player;
    [SerializeField] private new Transform renderer;
    [SerializeField] private float flipSpeed;

    private bool _canBounce;
    private bool _flipping;

    void Awake()
    {
        Player.Landed.AddListener(OnLanded);
        Player.Jumped.AddListener(OnJumped);
        Player.HitEntity.AddListener(OnHitEntity);
    }

    private void OnLanded()
    {
        _canBounce = true;
        StopBouncing();
    }

    private void OnJumped()
    {
        _canBounce = true;
        StopBouncing();
    }
    
    private void OnHitEntity()
    {
        _canBounce = true;
        StopBouncing();
    }

    public bool CanBounce()
    {
        return _canBounce;
    }

    public void Bounce()
    {
        player.Launch(Player.Stats.BounceForce);
        _canBounce = false;
        StartCoroutine(FlipAnimation());
    }

    private IEnumerator FlipAnimation()
    {
        _flipping = true;
        while (_flipping)
        {
            renderer.rotation = Quaternion.Euler(0, 0, renderer.rotation.eulerAngles.z - flipSpeed);
            yield return null;
        }
    }

    private void StopBouncing()
    {
        _flipping = false;
        renderer.rotation = Quaternion.Euler(0, 0, 0);
    }
}