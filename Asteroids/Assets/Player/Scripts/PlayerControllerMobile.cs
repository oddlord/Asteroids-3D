using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMobile : PlayerController {

    #region Private attributes
    private SimpleTouchController joystick;
    #endregion

    #region Start
    protected override void Start()
    {
        base.Start();
        joystick = GameManager.Instance.GetJoystick();
    }
    #endregion

    #region Get Inputs
    protected override void GetMovementInputs()
    {
        movementDirection = new Vector2(joystick.GetTouchPosition.x, joystick.GetTouchPosition.y);
    }

    protected override bool GetShootInput()
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
