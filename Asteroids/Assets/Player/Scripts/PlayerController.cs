using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Thrust")]
    [SerializeField]
    private float thrust = 500f;

    [Header("Projectiles")]
    [SerializeField]
    private GameObject projectileEmitter;
    [SerializeField]
    private float shootForce = 2000f;
    #endregion

    #region Private attributes
    private Rigidbody rb;
    #endregion

    #region Start, Update and FixedUpdate
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CanPerformAction() && InputManager.Instance.GetShoot())
        { 
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (CanPerformAction())
        {
            Vector2 movementDirection = InputManager.Instance.GetMovementDirection();
            if (movementDirection.magnitude > 0)
            {
                movementDirection.Normalize();
                ApplyRotation(movementDirection);
                ApplyThrust(movementDirection);
            }
        }
    }
    #endregion

    #region Utility functions
    private bool CanPerformAction()
    {
        bool playing = GameManager.Instance.GetGameState() == GameManager.GameState.Playing;
        bool alive = !PlayerManager.Instance.IsDead();
        return (playing && alive);
    }
    #endregion

    #region Shooting
    private void Shoot()
    {
        GameObject projectile = PoolsManager.Instance.GetProjectilesPool().GetAvailable();
        projectile.transform.position = projectileEmitter.transform.position;
        projectile.transform.rotation = projectileEmitter.transform.rotation;

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        // this is so the momentum of the ship is passed to the projectiles
        // so a faster ship means faster projectiles
        projectileRB.velocity = rb.velocity;
        projectileRB.angularVelocity = Vector3.zero;

        projectile.SetActive(true);

        projectileRB.AddForce(transform.up * shootForce);
    }
    #endregion

    #region Movement
    private void ApplyRotation(Vector2 movementDirection)
    {
        float heading = Mathf.Atan2(movementDirection.x, movementDirection.y);
        transform.rotation = Quaternion.AngleAxis(heading * Mathf.Rad2Deg, Vector3.back);
    }

    private void ApplyThrust(Vector2 movementDirection)
    {
        rb.AddForce(movementDirection * thrust);
        rb.angularVelocity = Vector3.zero;
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    #endregion
}
