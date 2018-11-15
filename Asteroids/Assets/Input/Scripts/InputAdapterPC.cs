using UnityEngine;

public class InputAdapterPC : InputAdapter
{
    #region Get Inputs
    public override Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public override bool GetShootInput()
    {
        return Input.GetButtonDown("Fire1");
    }
    #endregion
}
