using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider: MonoBehaviour {

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetAsteroidTag())
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
