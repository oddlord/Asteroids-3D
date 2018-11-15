using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ProjectilePool))]
public class PlayerController : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Thrust")]
    [SerializeField]
    protected float thrust = 750f;

    [Header("Projectiles")]
    [SerializeField]
    private GameObject projectileEmitter;
    [SerializeField]
    private float shootForce = 2000f;
    #endregion

    #region Private attributes
    private ProjectilePool projectilePool;
    #endregion

    #region Protected attributes
    protected PlayerManager playerManager;
    protected Rigidbody rb;
    protected Vector2 movementDirection;
    
    protected float thrustingForce;
    #endregion

    #region Start, Update and FixedUpdate
    protected virtual void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();

        projectilePool = GetComponent<ProjectilePool>();

        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);
        
        thrustingForce = 0f;
    }

    private void Update()
    {
        if (!playerManager.IsDead())
        {
            GetMovementInputs();
            movementDirection.Normalize();
            if (GetShootInput())
            {
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        ApplyRotation();
        ApplyThrust();
    }
    #endregion

    protected virtual void GetMovementInputs() { }
    protected virtual bool GetShootInput() { return false; }

    private void ApplyRotation()
    {
        float heading = Mathf.Atan2(movementDirection.x, movementDirection.y);
        transform.rotation = Quaternion.AngleAxis(heading * Mathf.Rad2Deg, Vector3.back);
    }

    #region Shooting
    private void Shoot()
    {
        GameObject projectile = projectilePool.GetAvailable();
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
    public void StopMovement()
    {
        movementDirection = new Vector2(0f, 0f);
        thrustingForce = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void ApplyThrust()
    {
        rb.AddForce(movementDirection * thrustingForce);
    }
    #endregion
}
