using UnityEngine;

public class PhysicsPauser : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private Vector2 _currentVelocity;
    
    void Awake()
    {
        GameManager.GamePause.AddListener(OnGamePause);
        GameManager.GameUnpause.AddListener(OnGameResume);
    }
    
    private void OnGamePause()
    {
        _currentVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
    
    private void OnGameResume()
    {
        rb.isKinematic = false;
        rb.velocity = _currentVelocity;
    }
}