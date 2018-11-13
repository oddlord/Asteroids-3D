using UnityEngine;

public class PlayerCollider : MonoBehaviour {

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerManager.IsDead() && !playerManager.IsSpawned() && other.gameObject.tag == "Asteroid")
        {
            playerManager.Die();
        }
    }
}
