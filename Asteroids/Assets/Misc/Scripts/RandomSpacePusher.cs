using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    public void SetRandomPush(float pushForce, float angularVelocity)
    {
        this.pushForce = pushForce;
        this.angularVelocity = angularVelocity;
    }

    public void SetMomentum(Vector3 momentum, float momentumDampeningFactor)
    {
        this.momentum = momentum;
        this.momentumDampeningFactor = momentumDampeningFactor;
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
