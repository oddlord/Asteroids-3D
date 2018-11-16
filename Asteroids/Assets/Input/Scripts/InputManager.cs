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
    private InputAdapter playerInput;
    private SimpleTouchController virtualJoystick;
    #endregion

    #region Enums
    enum Device
    {
        PC,
        MOBILE
    }
    #endregion

    #region Start
    void Start()
    {
		switch (device)
        {
            case Device.PC:
                playerInput = gameObject.AddComponent<InputAdapterPC>();
                break;
            case Device.MOBILE:
                GameObject joystick = Instantiate(virtualJoystickPrefab);
                virtualJoystick = joystick.GetComponent<SimpleTouchController>();
                UIManager.Instance.AddElement(joystick);
                playerInput = gameObject.AddComponent<InputAdapterMobile>();
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
    #endregion
}
