using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpacePusher : MonoBehaviour
{
    #region Private attributes
    private float pushForce = 0f;
    private float angularVelocity = 0f;

    // this is to transfer the velocity of the exploded object to its fragments
    private Vector3 momentum = Vector3.zero;
    // this is to adjust how much velocity to transfer (0=none, 1=all)
    private float momentumDampeningFactor = 0f;
    #endregion

    #region Setters
    public void SetRandomPush(float force, float aVel)
    {
        pushForce = force;
        angularVelocity = aVel;
    }

    public void SetMomentum(Vector3 m, float mDampeningFactor)
    {
        momentum = m;
        momentumDampeningFactor = mDampeningFactor;
    }
    #endregion

    #region Random Push
    public void GivePush()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.angularVelocity = Random.insideUnitSphere * angularVelocity;

        Vector3 randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();

        rb.velocity = (momentum * momentumDampeningFactor);
        rb.AddForce(randomDirection * pushForce);
    }
    #endregion
}
