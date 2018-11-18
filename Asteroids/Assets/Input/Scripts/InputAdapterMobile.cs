using UnityEngine;

public class InputAdapterMobile : IInputAdapter
{
    #region Private attributes
    private SimpleTouchController joystick;
    #endregion

    #region Constructor
    public InputAdapterMobile()
    {
        joystick = InputManager.Instance.GetJoystick();
    }
    #endregion

    #region Get Inputs
    public Vector2 GetMovementInput()
    {
        return new Vector2(joystick.GetTouchPosition.x, joystick.GetTouchPosition.y);
    }

    public bool GetShootInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            bool touchedJoystick = RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<RectTransform>(), touch.position);
            bool touchBegan = touch.phase == TouchPhase.Began;
            if (!touchedJoystick && touchBegan)
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}
