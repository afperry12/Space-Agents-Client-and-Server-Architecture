using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public CapsuleCollider collider;
    private float capsuleHalfHeight;
    
    private void Update()
    {
        
    }

    public void Start()
    {
        capsuleHalfHeight = collider.height / 2;
    }

    private void FixedUpdate()
    {
        SendMovementInputToServer();
        SendRotationInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendMovementInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            InAir(),
            Input.GetKey(KeyCode.R),
            Input.GetKey(KeyCode.Space),
        };
        
        NetworkSend.PlayerMovement(_inputs);
    }

    private void SendRotationInputToServer()
    {
        float[] _mouseInputs = new float[]
        {
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        };
        NetworkSend.PlayerRotation(_mouseInputs);
    }

    public bool InAir()
    {
        Physics.Raycast(collider.bounds.center, Vector3.down, out var hit);
        if (hit.distance > (capsuleHalfHeight + 10f))
        {
            return true;
        }
        else
        {
            return false;
        }
        
        // if (Physics.CheckCapsule(transform.position+collider.bounds.center,
        //     new Vector3(transform.position.x+collider.bounds.center.x, transform.position.y+collider.bounds.min.y - 0.1f,transform.position.z+ collider.bounds.center.z), 0.18f))
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
    }
    

}