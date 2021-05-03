using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;

public class CameraMovement : MonoBehaviour
{
    public Camera playerCamera;
    public SpriteRenderer mapRenderer;
    private Vector3 dragOrigin;

    public GameObject playerUI;
    public GameObject tilemapVisual;

    private float cameraSpeed;

    private float zoomStep;
    private float minCameraSize;
    private float maxCameraSize;

    private float mapMinX;
    private float mapMinY;
    private float mapMaxX;
    private float mapMaxY;

    void Start()
    {
        zoomStep = 1;
        minCameraSize = 1;
        maxCameraSize = 10;

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;

        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    void Update()
    {
        MoveCamera();
        PanCamera();
        WheelZoom();

        // Hide player UI with F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (playerUI.activeSelf == false)
            {
                playerUI.SetActive(true);
                tilemapVisual.SetActive(true);
            }
            else
            {
                playerUI.SetActive(false);
                tilemapVisual.SetActive(false);
            }
        }
    }

    private void MoveCamera()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);

        transform.position += moveDirection * cameraSpeed;

        playerCamera.transform.position = ClampCamera(playerCamera.transform.position);
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - playerCamera.ScreenToWorldPoint(Input.mousePosition);

            playerCamera.transform.position = ClampCamera(playerCamera.transform.position + difference);
        }
    }

    public void ZoomIn()
    {
        float newSize = playerCamera.orthographicSize - zoomStep;
        playerCamera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

        playerCamera.transform.position = ClampCamera(playerCamera.transform.position);

        cameraSpeed = playerCamera.orthographicSize * 0.0175f;
    }

    public void ZoomOut()
    {
        float newSize = playerCamera.orthographicSize + zoomStep;
        playerCamera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

        playerCamera.transform.position = ClampCamera(playerCamera.transform.position);

        cameraSpeed = playerCamera.orthographicSize * 0.0175f;
    }

    private void WheelZoom()
    {
        if (Input.mouseScrollDelta.y > 0 && UtilitiesClass.IsPointerOverUIObject() == false)
        {
            float newSize = playerCamera.orthographicSize - zoomStep;
            playerCamera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

            playerCamera.transform.position = ClampCamera(playerCamera.transform.position);
        }

        if (Input.mouseScrollDelta.y < 0 && UtilitiesClass.IsPointerOverUIObject() == false)
        {
            float newSize = playerCamera.orthographicSize + zoomStep;
            playerCamera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

            playerCamera.transform.position = ClampCamera(playerCamera.transform.position);
        }

        cameraSpeed = playerCamera.orthographicSize * 0.0175f;
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float cameraHeight = playerCamera.orthographicSize;
        float cameraWidth = playerCamera.orthographicSize * playerCamera.aspect;

        float minX = mapMinX + cameraWidth;
        float minY = mapMinY + cameraHeight;
        
        float maxX = mapMaxX - cameraWidth;
        float maxY = mapMaxY - cameraHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}