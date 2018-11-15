using UnityEngine;

[RequireComponent(typeof(AsteroidController))]
public class AsteroidCollider : MonoBehaviour
{

    #region Private attributes
    AsteroidController asteroidController;
    #endregion

    #region Start
    private void Start()
    {
        asteroidController = GetComponent<AsteroidController>();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetProjectileTag() || other.gameObject.tag == GameManager.Instance.GetPlayerShipTag())
        {
            asteroidController.Explode();
        }
    }
    #endregion
}
