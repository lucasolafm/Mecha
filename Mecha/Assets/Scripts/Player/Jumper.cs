using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Player _player;
    private bool _canJump;

    void Awake()
    {
        _player = GetComponent<Player>();
        
        Player.HitFloor.AddListener(OnHitFloor);
        Player.HitEntity.AddListener(OnHitEntity);
        
        _canJump = true;
    }

    private void OnHitEntity()
    {
        _canJump = false;
    }

    public void Tick()
    {
        if (!InputManager.I.Tapped || !_canJump) return;

        Jump();
    }

    private void OnHitFloor()
    {
        _canJump = true;
    }

    private void Jump()
    {
        return;
        _player.Launch(Player.Stats.JumpForce);
        Player.Jumped.Invoke();
        _canJump = false;
    }
}