using UnityEngine;

public class Mover : MonoBehaviour
{
    public new Transform transform;
    [SerializeField] private float maxSpeedChangePerc;
    [SerializeField] private float minSpeedChange;

    private float _currentSpeed;
    private int _direction;
    private float _targetSpeedChange;
    private float _maxSpeedChange;
    private float _speedChange;

    public void Tick()
    {
        if (GameManager.I.GameIsPaused) return;
        
        _direction = InputManager.I.HorizontalTilt - _currentSpeed > 0 ? 1 : -1;
        _targetSpeedChange = Mathf.Abs(InputManager.I.HorizontalTilt - _currentSpeed);
        _maxSpeedChange = Mathf.Max(Mathf.Abs(_currentSpeed) * maxSpeedChangePerc * Time.deltaTime, minSpeedChange * Time.deltaTime);
        _speedChange = Mathf.Min(_targetSpeedChange, _maxSpeedChange);

        _currentSpeed += _speedChange * _direction;
        transform.position += new Vector3(_currentSpeed * (Player.Stats.MoveSpeed * Time.deltaTime), 0);
    }
}