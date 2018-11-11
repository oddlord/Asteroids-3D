using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float thrust = 1000f;

    [SerializeField]
    private GameObject projectileEmitter;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float shootForce = 2000f;
    [SerializeField]
    private float shootRate = 0.25f; // how often, in secs, the laser can be shot
    [SerializeField]
    private float projectileLife = 1f;
    
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
        GameObject bullet = Instantiate(projectilePrefab, projectileEmitter.transform.position, projectileEmitter.transform.rotation * Quaternion.Euler(90, 0, 0)) as GameObject;
        bullet.transform.SetParent(playerManager.transform);

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * shootForce);

        Destroy(bullet, projectileLife);

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
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                bool touchedJoystick = RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<RectTransform>(), touch.position);
                bool touchBegan = touch.phase == TouchPhase.Began;
                if (!touchedJoystick && touchBegan)
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
