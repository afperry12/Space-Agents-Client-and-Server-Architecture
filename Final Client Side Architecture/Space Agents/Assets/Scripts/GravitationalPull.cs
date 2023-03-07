// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class GravitationalPull : MonoBehaviour
// {
//     const float G = .6674f;
//     public Rigidbody rb;
//
//     private void FixedUpdate()
//     {
//         GravitationalPull[] attractedObjects = FindObjectsOfType<GravitationalPull>();
//         foreach (GravitationalPull gravitationalpull in attractedObjects)
//         {
//             if (gravitationalpull != this)
//             {
//                 Gravity(gravitationalpull);
//             }
//             
//         }
//     }
//
//     void Gravity(GravitationalPull attractedObject)
//     {
//         Rigidbody attractedRb = attractedObject.rb;
//         Vector3 direction = rb.position - attractedRb.position;
//         float distance = direction.magnitude;
//
//         float forceMagnitude = G * (rb.mass * attractedRb.mass) / Mathf.Pow(distance, 2);
//         Vector3 force = direction.normalized * forceMagnitude;
//
//         attractedRb.AddForce(force);
//
//     }
//     
//     // Start is called before the first frame update
//     void Start()
//     {
//         
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//     }
// }
