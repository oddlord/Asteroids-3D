using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float thrust = 500f;

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

    private bool isMoving;
    private float thrustingForce;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        joystick = GameManager.Instance.GetJoystick();

        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);

        // shooting should be enabled right away
        lastShoot = -shootRate;

        isMoving = false;
        thrustingForce = 0f;
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
            GameManager.Device device = GameManager.Instance.GetDevice();
            switch (device)
            {
                case GameManager.Device.Mobile:
                    movementDirection = joystick.Direction;
                    isMoving = false;
                    if (movementDirection.magnitude > 0)
                    {
                        isMoving = true;
                        thrustingForce = movementDirection.magnitude * thrust;
                    }

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

                    break;

                case GameManager.Device.PC:
                    Vector2 mouseScreenPos = Input.mousePosition;
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z));
                    movementDirection = mouseWorldPos - (Vector2)(transform.position);

                    isMoving = false;
                    float cumulativeThrust = 0f;
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
                    {
                        isMoving = true;
                        cumulativeThrust += thrust;
                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        isMoving = true;
                        cumulativeThrust -= thrust;
                    }

                    if (isMoving)
                    {
                        thrustingForce = cumulativeThrust;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Shoot();
                    }

                    break;

                default:
                    Debug.LogError("Device not recognised!");
                    break;
            }

            movementDirection.Normalize();

            // the rotation does not depend on time
            // so it's safe to perform inside Update()
            float heading = Mathf.Atan2(movementDirection.x, movementDirection.y);
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(heading * Mathf.Rad2Deg, Vector3.up);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.AddForce(movementDirection * thrustingForce);
        }
    }
}
