using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private FixedJoystick joystick;
    [SerializeField]
    private GameObject laserEmitter;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float laserForce = 200f;
    [SerializeField]
    private float shootRate = 0.75f; // how often, in secs, the laser can be shot

    private float lastShoot;

    private void Start()
    {
        // shooting should be enabled right away
        lastShoot = Time.time - shootRate;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(laserPrefab, laserEmitter.transform.position, laserEmitter.transform.rotation) as GameObject;
        
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
