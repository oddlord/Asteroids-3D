using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMobile : PlayerController {

    #region Private attributes
    private FixedJoystick joystick;
    #endregion

    #region Start
    protected override void Start()
    {
        base.Start();
        joystick = GameManager.Instance.GetJoystick();
    }
    #endregion

    #region GetInputs
    protected override void GetInputs()
    {
        GameManager.Device device = GameManager.Instance.GetDevice();

        movementDirection = joystick.Direction;
        if (movementDirection.magnitude > 0)
        thrustingForce = movementDirection.magnitude * thrust;

        int touchCount = Input.touchCount;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            bool touchedJoystick = RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<RectTransform>(), touch.position);
            bool touchBegan = touch.phase == TouchPhase.Began;
            if (!touchedJoystick && touchBegan)
            {
                Shoot();
            }
        }
    }
    #endregion
}
