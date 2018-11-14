using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider: MonoBehaviour {

    #region Private attributes
    PlayerManager playerManager;
    #endregion

    #region Start
    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetAsteroidTag())
        {
            playerManager.UpdateScore(other.GetComponent<AsteroidController>().GetSize());
            Destroy(this.gameObject);
        }
    }
    #endregion
}
