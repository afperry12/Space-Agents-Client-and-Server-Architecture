// using System;
// using UnityEngine;
// using System.Collections;
//
// public class InputManager : MonoBehaviour
// {
//     public Keys pressedKey;
//     // public Keys wPressed;
//     // public Keys aPressed;
//     // public Keys sPressed;
//     // public Keys dPressed;
//     public float rotation;
//     
//     public float lookRateSpeed = 90f;
//     private Vector2 lookInput, screenCenter, mouseDistance;
//     private float rollInput;
//     public float rollSpeed = 90f, rollAcceleration = 3.5f;
//
//     public enum Keys
//     {
//         None,
//         W,
//         A,
//         S,
//         D
//     }
//
//     private void Start()
//     {
//         screenCenter.x = Screen.width * .5f;
//         screenCenter.y = Screen.height * .5f;
//         // wPressed = Keys.None;
//         // aPressed = Keys.None;
//         // sPressed = Keys.None;
//         // dPressed = Keys.None;
//         pressedKey = Keys.None;
//     }
//
//     private void Update()
//     {
//         CheckRotation();
//         CheckCamera();
//         CheckInput();
//         // rotation = GameManager.instance.UnwrapEulerAngles(transform.localEulerAngles.y);
//     }
//     
//     private void FixedUpdate()
//     {
//         CheckInput();
//     }
//
//     private void CheckCamera()
//     {
//         Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
//         Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
//         float rayLength;
//
//         if (groundPlane.Raycast(cameraRay, out rayLength))
//         {
//             Vector3 pointToLook = cameraRay.GetPoint(rayLength);
//             Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
//             
//             transform.LookAt(new Vector3(pointToLook.x, pointToLook.y, pointToLook.z));
//         }
//     }
//
//     private void CheckRotation()
//     {
//         // gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x,
//         //     GameManager.instance.WrapEulerAngles(rotation), gameObject.transform.localEulerAngles.z);
//         
//         // NetworkSend.SendPlayerRotation(rotation);
//         
//         // lookInput.x = Input.mousePosition.x;
//         // lookInput.y = Input.mousePosition.y;
//         //
//         // mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
//         // mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;
//         //
//         // mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
//         //
//         // rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);
//         //
//         // transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);
//     }
//
//     // private void CheckInput()
//     // {
//     //     if (Input.GetKeyDown(KeyCode.W))
//     //     {
//     //         wPressed = Keys.W;
//     //     }
//     //     else if (Input.GetKeyUp(KeyCode.W))
//     //     {
//     //         wPressed = Keys.None;
//     //     }
//     //
//     //     if (Input.GetKeyDown(KeyCode.A))
//     //     {
//     //         aPressed = Keys.A;
//     //     }
//     //     else if (Input.GetKeyUp(KeyCode.A))
//     //     {
//     //         aPressed = Keys.None;
//     //     }
//     //
//     //     if (Input.GetKeyDown(KeyCode.S))
//     //     {
//     //         sPressed = Keys.S;
//     //     }
//     //     else if (Input.GetKeyUp(KeyCode.S))
//     //     {
//     //         sPressed = Keys.None;
//     //     }
//     //
//     //     if (Input.GetKeyDown(KeyCode.D))
//     //     {
//     //         dPressed = Keys.D;
//     //     }
//     //     else if (Input.GetKeyUp(KeyCode.D))
//     //     {
//     //         dPressed = Keys.None;
//     //     }
//     //
//     //
//     //     NetworkSend.SendKeyInput(wPressed,aPressed,sPressed,dPressed);
//     // }
//     private void CheckInput()
//     {
//         // if (Input.GetKeyDown(KeyCode.W))
//         // {
//         //     pressedKey = Keys.W;
//         // }
//         // else if (Input.GetKeyUp(KeyCode.W))
//         // {
//         //     pressedKey = Keys.None;
//         // }
//         //
//         // if (Input.GetKeyDown(KeyCode.A))
//         // {
//         //     pressedKey = Keys.A;
//         // }
//         // else if (Input.GetKeyUp(KeyCode.A))
//         // {
//         //     pressedKey = Keys.None;
//         // }
//         //
//         // if (Input.GetKeyDown(KeyCode.S))
//         // {
//         //     pressedKey = Keys.S;
//         // }
//         // else if (Input.GetKeyUp(KeyCode.S))
//         // {
//         //     pressedKey = Keys.None;
//         // }
//         //
//         // if (Input.GetKeyDown(KeyCode.D))
//         // {
//         //     pressedKey = Keys.D;
//         // }
//         // else if (Input.GetKeyUp(KeyCode.D))
//         // {
//         //     pressedKey = Keys.None;
//         // }
//         //
//         //
//         // NetworkSend.SendKeyInput(pressedKey);
//
//         bool[] inputs = new bool[]
//         {
//             Input.GetKey(KeyCode.W),
//             Input.GetKey(KeyCode.A),
//             Input.GetKey(KeyCode.S),
//             Input.GetKey(KeyCode.D),
//         };
//         
//         NetworkSend.SendKeyInput(inputs);
//
//     }
// }