using UnityEngine;

public class RandomMover : MonoBehaviour
{
    private float angularVelocity;
    private float velocity;

    // this is to keep transfer the velocity of the ship to its fragments when it explodes
    private Vector3 baseVelocity = Vector3.zero;
    // this is to adjust how much velocity to transfer (0=none, 1=all)
    private float baseVelocityDampeningFactor = 0.5f;

    public void SetAngularVelocity(float t)
    {
        angularVelocity = t;
    }

    public void SetVelocity(float v)
    {
        velocity = v;
    }

    public void SetBaseVelocity(Vector3 bVel)
    {
        baseVelocity = bVel;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.angularVelocity = Random.insideUnitSphere * angularVelocity;

        Vector3 randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();

        rb.velocity = (baseVelocity * baseVelocityDampeningFactor) + (randomDirection * velocity);
    }
}