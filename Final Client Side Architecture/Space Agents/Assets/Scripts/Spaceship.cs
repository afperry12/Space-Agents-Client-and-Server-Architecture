// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Spaceship : MonoBehaviour
// {
//     public float forward = 25f, strafe = 7.5f, hover = 5f;
//     private float activeForward, activeStrafe, activeHover;
//     private float forwardAccel = 2.5f, strafeAccel = 2f, hoverAccel = 2f;
//
//     public float lookRateSpeed = 90f;
//
//     private Vector2 lookInput, screenCenter, mouseDistance;
//
//     private float rollInput;
//     public float rollSpeed = 90f, rollAcceleration = 3.5f;
//     
//     // Start is called before the first frame update
//     void Start()
//     {
//         screenCenter.x = Screen.width * .5f;
//         screenCenter.y = Screen.height * .5f;
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         lookInput.x = Input.mousePosition.x;
//         lookInput.y = Input.mousePosition.y;
//
//         mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
//         mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;
//
//         mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
//
//         rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);
//         
//         transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);
//         
//         activeForward = Mathf.Lerp(activeForward, Input.GetAxisRaw("Vertical") * forward, forwardAccel * Time.deltaTime);
//         activeStrafe = Mathf.Lerp(activeStrafe, Input.GetAxisRaw("Horizontal") * strafe, strafeAccel * Time.deltaTime);
//         activeHover = Mathf.Lerp(activeHover, Input.GetAxisRaw("Hover") * hover, hoverAccel * Time.deltaTime);
//
//         transform.position += (transform.forward * activeForward * Time.deltaTime);
//         transform.position += (transform.right * activeStrafe * Time.deltaTime) +
//                               (transform.forward * activeHover * Time.deltaTime);
//         if (Input.GetMouseButton(0))
//         {
//             transform.Translate(transform.forward * -25f * Time.deltaTime);
//         }
//         if (Input.GetMouseButton(1))
//         {
//             transform.Translate(transform.forward * 25f * Time.deltaTime);
//         }
//     }
// }
