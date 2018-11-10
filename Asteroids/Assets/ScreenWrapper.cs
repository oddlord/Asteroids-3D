using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{

    private Renderer[] renderers;

    private bool isWrappingX;
    private bool isWrappingY;
    Camera cam;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        isWrappingX = false;
        isWrappingY = false;
        cam = Camera.main;
    }
	
    bool CheckRenderers()
    {
        foreach (var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }
        
        return false;
    }

    void ScreenWrap()
    {
        bool isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }

    void Update()
    {
        ScreenWrap();
    }
}
