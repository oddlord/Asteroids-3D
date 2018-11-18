using UnityEngine;

public interface IInputAdapter
{
    #region Get Inputs
    Vector2 GetMovementInput();
    bool GetShootInput();
    #endregion
}
