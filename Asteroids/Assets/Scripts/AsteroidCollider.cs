using UnityEngine;

[RequireComponent(typeof(AsteroidManager))]
public class AsteroidCollider : MonoBehaviour {

    AsteroidManager asteroidManager;

    private void Start()
    {
        asteroidManager = GetComponent<AsteroidManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetProjectileTag())
        {
            asteroidManager.Explode();
        }
    }
}
