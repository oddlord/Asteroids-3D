using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    #region Private attributes
    private Camera cam;
    #endregion

    #region Start and Update
    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
        transform.position = cam.ViewportToWorldPoint(new Vector3(Mathf.Repeat(viewportPosition.x, 1f), Mathf.Repeat(viewportPosition.y, 1f), Mathf.Abs(transform.position.z - cam.transform.position.z)));
    }
    #endregion
}
