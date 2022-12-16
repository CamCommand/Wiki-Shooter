using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager instance
    {
        get { return _instance; }
    }

    private PlayerInput _playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _playerControls = new PlayerInput();
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 GetMouseDelta()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerFired()
    {
        return _playerControls.Player.Fire.triggered;
    }

    public bool PlayerAiming()
    {
        return _playerControls.Player.Aim.IsPressed();
    }
}
