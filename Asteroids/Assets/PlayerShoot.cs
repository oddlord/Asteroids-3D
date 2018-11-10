using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private FixedJoystick joystick;

    private void Shoot()
    {
        Debug.Log("Shooting!!");
    }

    void Update()
    {
        int touchCount = Input.touchCount;
        if (touchCount > 1 || (touchCount == 1 && !joystick.isDragging()))
        {
            Shoot();
        }
    }
}
