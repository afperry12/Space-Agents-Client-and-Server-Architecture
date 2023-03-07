using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;
    
    private float verticalRotation;
    private float horizontalRotation;
    
    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ToggleCursorMode();
        }
    
        // if (Cursor.lockState == CursorLockMode.Locked)
        // {
            Look();
        // }
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }
    
    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");
    
        verticalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;
    
        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);
    
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }
    //
    // private void ToggleCursorMode()
    // {
    //     Cursor.visible = !Cursor.visible;
    //
    //     if (Cursor.lockState == CursorLockMode.None)
    //     {
    //         Cursor.lockState = CursorLockMode.Locked;
    //     }
    //     else
    //     {
    //         Cursor.lockState = CursorLockMode.None;
    //     }
    // }
    
    
    
    // public float mouseSensitivity = 2f;
    // public GameObject cameraPivot;
    // private float cameraRotateX = 0f;
    //
    // void Start()
    // {
    //
    //     //--hide the mosue cursor. Press Esc during play to show the cursor. --
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }
    //
    // void Update()
    // {
    //     //--get values used for character and camera movement--
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     float verticalInput = Input.GetAxis("Vertical");
    //     float mouse_X = Input.GetAxis("Mouse X") * mouseSensitivity;
    //     float mouse_Y = Input.GetAxis("Mouse Y") * mouseSensitivity;
    //     float normalizedSpeed = Vector3.Dot(new Vector3(horizontalInput, 0f, verticalInput).normalized, new Vector3(horizontalInput, 0f, verticalInput).normalized);
    //     
    //     cameraRotateX += mouse_Y;
    //     cameraRotateX = Mathf.Clamp(cameraRotateX, -15, 60); //limites the up/down rotation of the camera 
    //     cameraPivot.transform.localRotation = Quaternion.Euler(cameraRotateX, 0, 0);
    // }

}