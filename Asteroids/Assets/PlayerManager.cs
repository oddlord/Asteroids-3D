using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController playerController;
    
    private bool dead;
    private int score;
    
    void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        
        dead = false;
        score = 0;
	}

    public bool isDead()
    {
        return dead;
    }

    public void Die()
    {
        GameObject playerShip = transform.GetChild(0).gameObject;

        if (playerShip.tag != GameManager.Instance.GetPlayerShipTag())
        {
            Debug.LogError("ERROR: no GameObject with tag " + GameManager.Instance.GetPlayerShipTag() + " found!");
        }

        Rigidbody playerShipRB = playerShip.GetComponent<Rigidbody>();

        for (int i = 0; i < playerShip.transform.childCount; i++)
        {
            GameObject child = playerShip.transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                Rigidbody childRB = child.AddComponent<Rigidbody>();
                childRB.useGravity = false;
                childRB.drag = 0f;
                childRB.constraints = RigidbodyConstraints.FreezePositionZ;
                RandomMover rm = child.AddComponent<RandomMover>();
                rm.SetTumble(1f);
                rm.SetVelocity(15f);
                rm.SetBaseVelocity(playerShipRB.velocity);
                child.AddComponent<ScreenWrapper>();
            }
        }

        playerController.StopMovement();

        dead = true;
    }

    public void UpdateScore(int size)
    {
        score += size*10;
        GameManager.Instance.UpdateScore(score);
    }
}
