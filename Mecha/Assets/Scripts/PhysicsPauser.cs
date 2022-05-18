using UnityEngine;

public class PhysicsPauser : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private Vector2 _currentVelocity;
    
    void Awake()
    {
        GameManager.GamePause.AddListener(OnGamePause);
        GameManager.GameUnpause.AddListener(OnGameUnpause);
    }
    
    private void OnGamePause()
    {
        _currentVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
    
    private void OnGameUnpause()
    {
        rb.isKinematic = false;
        rb.velocity = _currentVelocity;
    }
}