using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            PlayerManager.Instance.Die();
        }
    }
    #endregion
}
