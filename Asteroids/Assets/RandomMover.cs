using UnityEngine;

public class RandomMover : MonoBehaviour
{
    private float tumble;
    private float velocity;

    private Vector3 baseVelocity = Vector3.zero;
    private float baseVelocityDampeningFactor = 0.5f;

    public void SetTumble(float t)
    {
        tumble = t;
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

        rb.angularVelocity = Random.insideUnitSphere * tumble;

        Vector3 randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();

        rb.velocity = (baseVelocity * baseVelocityDampeningFactor) + (randomDirection * velocity);
    }
}