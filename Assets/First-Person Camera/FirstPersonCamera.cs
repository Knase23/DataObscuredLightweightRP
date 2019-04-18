using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    [Tooltip("Determines whether the player can move camera or not.")]
    public bool enableCameraMovement;
    [Tooltip("from center, how much - range in degrees - does the camera have to move up and down.")]
    [Range(0, 180)]
    public float rotationRange = 170;
    [Tooltip("Determines how sensitive the mouse is.")]
    [Range(0.01f, 35)]
    public float mouseSensitivity = 10f;
    [Tooltip("Mouse Smoothness.")]
    [Range(1, 100)]
    public float cameraSmoothing = 5f;
    [Tooltip("Camera that you wish to rotate.")]
    public Transform playerCamera;
    internal Vector3 cameraStartingPosition;


    [HideInInspector]
    public Vector3 targetAngles;
    private Vector3 followAngles;
    private Vector3 followVelocity;
    private Vector3 originalRotation;

    private void Awake()
    {
        originalRotation = transform.localRotation.eulerAngles;
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraStartingPosition = playerCamera.localPosition;
    }

    // Update is called once per frame
    public void LookAround(float xInput, float yInput)
    {
        if (enableCameraMovement)
        {
            float mouseXInput;
            float mouseYInput;
            mouseXInput = yInput;
            mouseYInput = xInput;
            if (targetAngles.y > 180) { targetAngles.y -= 360; followAngles.y -= 360; } else if (targetAngles.y < -180) { targetAngles.y += 360; followAngles.y += 360; }
            if (targetAngles.x > 180) { targetAngles.x -= 360; followAngles.x -= 360; } else if (targetAngles.x < -180) { targetAngles.x += 360; followAngles.x += 360; }
            targetAngles.y += mouseYInput * mouseSensitivity;
            targetAngles.x += mouseXInput * mouseSensitivity;
            targetAngles.y = Mathf.Clamp(targetAngles.y, -0.5f * Mathf.Infinity, 0.5f * Mathf.Infinity);
            targetAngles.x = Mathf.Clamp(targetAngles.x, -0.5f * rotationRange, 0.5f * rotationRange);
            followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, cameraSmoothing / 100);
            playerCamera.localRotation = Quaternion.Euler(-followAngles.x + originalRotation.x, 0, 0);
            transform.localRotation = Quaternion.Euler(0, followAngles.y + originalRotation.y, 0);
        }
    }
}
