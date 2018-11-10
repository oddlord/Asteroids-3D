using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private float thrust = 100f;

    Rigidbody rb;
    private Vector2 movementDirection;

	void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector2(0, 0);
	}
	
	void Update()
    {
        movementDirection = joystick.Direction;
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
