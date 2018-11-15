using UnityEngine;

public class InputAdapterMobile : InputAdapter
{
    #region Private attributes
    private SimpleTouchController joystick;
    #endregion

    #region Start
    private void Awake()
    {
        joystick = InputManager.Instance.GetJoystick();
    }
    #endregion

    #region Get Inputs
    public override Vector2 GetMovementInput()
    {
        return new Vector2(joystick.GetTouchPosition.x, joystick.GetTouchPosition.y);
    }

    public override bool GetShootInput()
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
