using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private FixedJoystick joystick;

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

    Rigidbody rb;
    private Vector2 movementDirection;

    private float lastShoot;

    private bool dead;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);

        // shooting should be enabled right away
        lastShoot = Time.time - shootRate;

        dead = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(laserPrefab, laserEmitter.transform.position, laserEmitter.transform.rotation * Quaternion.Euler(90, 0, 0)) as GameObject;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * laserForce);

        Destroy(bullet, 3f);

        lastShoot = Time.time;
    }

    private float GetLastShootDeltaTime()
    {
        return (Time.time - lastShoot);
    }

    void Update()
    {
        if (!dead)
        {
            movementDirection = joystick.Direction;

            int touchCount = Input.touchCount;
            if (touchCount > 1 || (touchCount == 1 && !joystick.isDragging()))
            {
                if (GetLastShootDeltaTime() >= shootRate)
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

    private void Die()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Rigidbody childRB = child.AddComponent<Rigidbody>();
            childRB.useGravity = false;
            childRB.drag = 0f;
            childRB.constraints = RigidbodyConstraints.FreezePositionZ;
            RandomMover rm = child.AddComponent<RandomMover>();
            rm.SetTumble(1f);
            rm.SetVelocity(0f);
            rm.SetBaseVelocity(rb.velocity);
            child.AddComponent<ScreenWrapper>();
        }

        movementDirection = new Vector2(0f, 0f);

        dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead && other.gameObject.tag == "Asteroid")
        {
            Die();
        }
    }
}
