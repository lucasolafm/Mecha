using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float leanHorizontalMax;
    [SerializeField] private float leanVerticalMax;
    [SerializeField] private float movementToLeanX;
    [SerializeField] private float movementToLeanY;
    [SerializeField] private float smoothingX;
    [SerializeField] private float smoothingY;
    
    private float _posX;
    private float _posY;
    private float _minHeight;
    private float _leanMultiplierY;

    void Awake()
    {
        _minHeight = transform.position.y;
    }

    private float highest;
    public void Tick()
    {
        if (GameManager.I.GameIsPaused) return;    
        
        _posX = player.transform.position.x + 
                Mathf.Clamp(player.GetMovement().x / movementToLeanX, -1, 1) * leanHorizontalMax;

        transform.position += new Vector3((_posX - transform.position.x) * smoothingX, 0);
    }

    public void FixedTick()
    {
        if (GameManager.I.GameIsPaused) return;

        _leanMultiplierY = Mathf.Clamp(player.GetPhysicsMovement().y / movementToLeanY, -1, 1);

        _posY = Mathf.Max(player.transform.position.y + _leanMultiplierY * leanVerticalMax, _minHeight);
        
        transform.position += new Vector3(0, (_posY - transform.position.y) * smoothingY);
    }
}
