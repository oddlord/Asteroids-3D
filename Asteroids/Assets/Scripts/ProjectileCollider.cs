using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider: MonoBehaviour {

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetAsteroidTag())
        {
            playerManager.UpdateScore(other.GetComponent<AsteroidManager>().GetSize());
            Destroy(this.gameObject);
        }
    }
}
