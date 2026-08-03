using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragSpeed = 1f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    private void HandleDrag()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // Continue dragging
        if (Input.GetMouseButton(1))
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPosition;

            transform.position += difference * dragSpeed;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll == 0f)
            return;

        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}