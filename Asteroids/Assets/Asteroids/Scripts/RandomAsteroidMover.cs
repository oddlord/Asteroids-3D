using UnityEngine;

public class RandomAsteroidMover : MonoBehaviour
{
    [SerializeField]
    private float tumble = 0.25f;
    [SerializeField]
    private float velocity = 1f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.angularVelocity = Random.insideUnitSphere * tumble;

        Vector3 randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();

        rb.velocity = randomDirection * velocity;
    }
}