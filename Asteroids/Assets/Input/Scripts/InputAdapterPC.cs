using UnityEngine;

public class InputAdapterPC : IInputAdapter
{
    #region Get Inputs
    public Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public bool GetShootInput()
    {
        return Input.GetButtonDown("Fire1");
    }
    #endregion
}
