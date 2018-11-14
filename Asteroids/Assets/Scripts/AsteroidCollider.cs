using UnityEngine;

[RequireComponent(typeof(AsteroidController))]
public class AsteroidCollider : MonoBehaviour {

    #region Private attributes
    AsteroidController asteroidManager;
    #endregion

    #region Start
    private void Start()
    {
        asteroidManager = GetComponent<AsteroidController>();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetProjectileTag())
        {
            asteroidManager.Explode();
        }
    }
    #endregion
}
