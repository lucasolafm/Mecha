using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager I;

    [HideInInspector] public float HorizontalTilt;

    [SerializeField] private new Camera camera;
    public RectTransform joystickBase;
    public RectTransform joystickStick;
    public float joystickSizeWorld;
    [SerializeField] private float joystickHeight;

    private float _joystickSize;
    private Vector2 _centerPos;
    private Touch _touch;

    void Awake()
    {
        I = this;
    }
    
    void Start()
    {
        _joystickSize = camera.WorldToScreenPoint(new Vector2(joystickSizeWorld, 0)).x - 
                        camera.WorldToScreenPoint(Vector2.zero).x;
        joystickBase.sizeDelta = new Vector2(_joystickSize * 2 + _joystickSize * 2 * joystickHeight, _joystickSize * 2 * joystickHeight);
        joystickStick.sizeDelta = new Vector2(_joystickSize * 2 * joystickHeight, _joystickSize * 2 * joystickHeight);
    }
    
    public void Tick()
    {
        HorizontalTilt = 0;
        joystickBase.gameObject.SetActive(false);
        joystickStick.gameObject.SetActive(false);

        if (GameManager.I.GameIsPaused) return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            HorizontalTilt = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            HorizontalTilt = 1;
        }
    
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                SetCenter(_touch.position);
            }

            HorizontalTilt = GetHorizontalTilt(_touch.position.x);
            SetStickPosition();
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetCenter(Input.mousePosition);
            }
        
            HorizontalTilt = GetHorizontalTilt(Input.mousePosition.x);
            SetStickPosition();
        }

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                SetCenter(Input.mousePosition);
            }
        }
    }
    
    private void SetCenter(Vector2 position)
    {
        _centerPos = position;
        joystickBase.position = _centerPos;
    }
    
    private void SetStickPosition()
    {
        joystickBase.gameObject.SetActive(true);
        joystickStick.gameObject.SetActive(true);
        joystickStick.position = _centerPos + new Vector2(HorizontalTilt * _joystickSize, 0);
    }

    private float GetHorizontalTilt(float pressPos)
    {
        return Mathf.Clamp((pressPos - _centerPos.x) / _joystickSize, -1, 1);
    }
}