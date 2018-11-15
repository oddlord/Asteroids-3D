using UnityEngine;

public class InputAdapter : MonoBehaviour
{
    #region Virtual methods
    public virtual Vector2 GetMovementInput() { return Vector2.zero; }
    public virtual bool GetShootInput() { return false; }
    #endregion
}
