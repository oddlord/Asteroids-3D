using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(Mathf.Repeat(transform.position.x, Screen.width), Mathf.Repeat(transform.position.y, Screen.height), 0f);
    }
}
