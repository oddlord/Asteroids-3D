using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ProjectilePool))]
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
    private PlayerManager playerManager;
    private FixedJoystick joystick;

    private ProjectilePool projectilePool;

    private Rigidbody rb;
    private Vector2 movementDirection;

    private bool isMoving;
    private float thrustingForce;
    #endregion

    #region Start, Update and FixedUpdate
    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        joystick = GameManager.Instance.GetJoystick();

        projectilePool = GetComponent<ProjectilePool>();

        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);

        isMoving = false;
        thrustingForce = 0f;
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
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(transform.position.z - Camera.main.transform.position.z)));
                    movementDirection = mouseWorldPos - (Vector2)(transform.position);

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
            transform.rotation = Quaternion.AngleAxis(heading * Mathf.Rad2Deg, Vector3.back);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.AddForce(movementDirection * thrustingForce);
            isMoving = false;
        }
    }
    #endregion

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
    #endregion
}
