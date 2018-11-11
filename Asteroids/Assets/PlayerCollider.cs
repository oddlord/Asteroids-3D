using UnityEngine;

public class PlayerCollider : MonoBehaviour {

    PlayerManager playerManager;

    public void SetPlayerManager(PlayerManager pm)
    {
        playerManager = pm;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerManager.IsDead() && !playerManager.IsSpawned() && other.gameObject.tag == "Asteroid")
        {
            playerManager.Die();
        }
    }
}
