using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton pattern
    private static InputManager instance;

    public static InputManager Instance { get { return instance; } }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
    #endregion

    #region SerializeField attributes
    [Header("Joystick")]
    [SerializeField]
    private GameObject virtualJoystickPrefab;

    [Header("Input mode")]
    [SerializeField]
    private Device device;
    #endregion

    #region Private attributes
    private IInputAdapter playerInput;
    private SimpleTouchController virtualJoystick;
    #endregion

    #region Enums
    enum Device
    {
        PC,
        Mobile
    }
    #endregion

    #region Start
    void Start()
    {
		switch (device)
        {
            case Device.PC:
                playerInput = new InputAdapterPC();
                break;
            case Device.Mobile:
                GameObject joystick = Instantiate(virtualJoystickPrefab);
                virtualJoystick = joystick.GetComponent<SimpleTouchController>();
                UIManager.Instance.AddVirtualJoystick(joystick);
                playerInput = new InputAdapterMobile();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Getters
    public Vector2 GetMovementDirection()
    {
        return playerInput.GetMovementInput();
    }

    public bool GetShoot()
    {
        return playerInput.GetShootInput();
    }

    public SimpleTouchController GetJoystick()
    {
        return virtualJoystick;
    }

    public bool IsMobile()
    {
        return device == Device.Mobile;
    }
    #endregion
}
