using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float thrust = 10000f;

    [SerializeField]
    private GameObject laserEmitter;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float laserForce = 20000f;
    [SerializeField]
    private float shootRate = 0.5f; // how often, in secs, the laser can be shot
    
    private PlayerManager playerManager;
    private FixedJoystick joystick;

    private Rigidbody rb;
    private Vector2 movementDirection;

    private float lastShoot;

    private void Start()
    {
        joystick = FixedJoystick.Instance;

        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);

        // shooting should be enabled right away
        lastShoot = -shootRate;
    }

    public void SetPlayerManager(PlayerManager pm)
    {
        playerManager = pm;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(laserPrefab, laserEmitter.transform.position, laserEmitter.transform.rotation * Quaternion.Euler(90, 0, 0)) as GameObject;
        bullet.transform.SetParent(playerManager.transform);

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * laserForce);

        Destroy(bullet, 3f);

        lastShoot = Time.time;
    }

    private float GetShootDeltaTime()
    {
        return (Time.time - lastShoot);
    }

    public void StopMovement()
    {
        movementDirection = new Vector2(0f, 0f);
    }

    void Update()
    {
        if (!playerManager.IsDead())
        {
            movementDirection = joystick.Direction;

            int touchCount = Input.touchCount;
            if (touchCount > 1 || (touchCount == 1 && !joystick.isDragged()))
            {
                if (GetShootDeltaTime() >= shootRate)
                {
                    Shoot();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementDirection.magnitude > 0)
        {
            rb.AddForce(movementDirection * thrust);

            float heading = Mathf.Atan2(movementDirection.x, movementDirection.y);
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(heading * Mathf.Rad2Deg, Vector3.up);
        }
    }
}
