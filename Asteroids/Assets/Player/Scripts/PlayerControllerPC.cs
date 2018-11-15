using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPC : PlayerController
{
    #region Get Inputs
    protected override void GetMovementInputs()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected override bool GetShootInput()
    {
        return Input.GetButtonDown("Fire1");
    }
    #endregion
}
