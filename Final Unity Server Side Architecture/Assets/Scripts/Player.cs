

using System;
using UnityEngine;
public class Player : MonoBehaviour
{
    public int connectionID;
    public string username;
    public bool inGame;
    public string Token;

    public GameObject playerModel;


    public string animation;

    public Vector3 position;
    public Quaternion rotation;

    private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
    private bool[] inputs;

    //private float yVelocity = 0;
    public float gravity = -9.8f;

    public float currentYaw;
    public float currentPitch;

    public float mass;

    // Add a canMove flag to control whether the player can move or not
    //private bool canMove = true;

    public void Initialize(int _id, string _username, Vector3 _spawnPosition, GameObject player)
    {
        connectionID = _id;
        username = _username;
        position = _spawnPosition;
        rotation = Quaternion.identity;
        playerModel = player;
        currentYaw = 0;
        currentPitch = 0;
        mass = MassCalculator.CalculateMass(playerModel);
        Debug.Log("Player mass: " + mass);
        inputs = new bool[4];
    }




    public void MovementUpdate(int connectionID)
    {
        Debug.Log("Movement Update called");
        // Initialize _inputDirection to zero
        Vector2 _inputDirection = Vector2.zero;

        // Update the animation based on the player's inputs
        animation = "idle";
        if (inputs[0])
        {
            animation = "walk";
            _inputDirection.y += 1;
            if (inputs[5])
            {
                moveSpeed = 10f / Constants.TICKS_PER_SEC;
                animation = "run";
            }
        }
        if (inputs[1])
        {
            animation = "walk";
            _inputDirection.y -= 1;
            if (inputs[5])
            {
                moveSpeed = 10f / Constants.TICKS_PER_SEC;
                animation = "run";
            }
        }
        if (inputs[2])
        {
            animation = "walk";
            _inputDirection.x += 1;
            if (inputs[5])
            {
                moveSpeed = 10f / Constants.TICKS_PER_SEC;
                animation = "run";
            }
        }
        if (inputs[3])
        {
            animation = "walk";
            _inputDirection.x -= 1;
            if (inputs[5])
            {
                moveSpeed = 10f / Constants.TICKS_PER_SEC;
                animation = "run";
            }
        }

        // Set downward to true if the player is pressing the jump button
        bool downward = inputs[4];

        // Call the Move method with the current input direction and the jump flag
        Dispatcher.RunOnMainThread(() =>
        {
            Move(_inputDirection, downward, connectionID);
        });
        Debug.Log("Movement Update finished");
    }

    public void Move(Vector2 inputDirection, bool downward, int connectionID)
    {
        // Get the player's Rigidbody component
        Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

        // Get the player's CapsuleCollider component
        CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

        // Get the player's current position and the planet's center position
        Vector3 playerPosition = playerModel.transform.position;
        Vector3 planetCenter = Vector3.zero;

        // Calculate the gravitational force on the player
        Vector3 gravityDirection = (planetCenter - playerPosition).normalized;
        Vector3 gravityForce = gravity * playerRigidbody.mass * -gravityDirection;

        // Apply the gravitational force to the player
        playerRigidbody.AddForce(gravityForce);

        // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
        Vector3 surfaceNormal = Vector3.zero;
        if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
        {
            surfaceNormal = hit.normal;
        }
        Quaternion surfaceRotation = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal);
        Vector3 moveDirection = surfaceRotation * new Vector3(inputDirection.x, 0f, inputDirection.y);
        moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

        // Calculate the maximum distance that the player can move in a single iteration
        float surfaceDistance = playerCollider.radius + 0.1f;
        float maxSpeed = 3f;
        float timestep = 0.1f;
        float maxDistance = maxSpeed * timestep;
        int iterations = Mathf.CeilToInt(surfaceDistance / maxDistance);

        // Move the player in small increments using CCD
        for (int i = 0; i < iterations; i++)
        {
            Debug.LogError("iter1");
            // Calculate the player's next position and rotation
            Vector3 nextPosition = playerPosition + moveDirection * maxSpeed * timestep;
            Quaternion nextRotation = surfaceRotation;
            Debug.LogError("iter2");
            // Use CCD to move the player's position and rotation
            playerRigidbody.MovePosition(nextPosition);
            playerRigidbody.MoveRotation(nextRotation);
            Debug.LogError("iter3");

            // Calculate the start and end points of the capsule collider based on the player's movement direction
            Vector3 capsuleStart = playerPosition - surfaceNormal * (playerCollider.height / 2f - playerCollider.radius);
            Vector3 capsuleEnd = playerPosition + moveDirection.normalized * (playerCollider.height / 2f - playerCollider.radius);

            // Draw a debug line to visualize the capsule collider
            Debug.DrawLine(capsuleStart, capsuleEnd, Color.red);


            // Check for collisions with the planet surface
            if (playerRigidbody.SweepTest(-surfaceNormal, out RaycastHit hitInfo, maxDistance, QueryTriggerInteraction.Ignore))
            {
                Debug.LogError("collision!!!");
                // Handle the collision here
                Vector3 adjustPosition = playerPosition + moveDirection * maxSpeed * timestep * hitInfo.distance;
                playerRigidbody.MovePosition(adjustPosition);
                playerRigidbody.velocity = Vector3.zero; // Reset velocity to prevent continued movement after a collision
            }
            Debug.LogError("iter4");
        }
        Debug.LogError("iter5");

        // Send the player's updated position to the client
        NetworkSend.PlayerPosition(connectionID, playerModel);
    }



    //ALSO FLIES THROUGH?
    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Get the player's current position and the planet's center position
    //    Vector3 playerPosition = playerModel.transform.position;
    //    Vector3 planetCenter = Vector3.zero;

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = (planetCenter - playerPosition).normalized;
    //    Vector3 gravityForce = gravity * playerRigidbody.mass * -gravityDirection;

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);

    //    // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
    //    Vector3 surfaceNormal = Vector3.zero;
    //    if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        surfaceNormal = hit.normal;
    //    }
    //    Quaternion surfaceRotation = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal);
    //    Vector3 moveDirection = surfaceRotation * new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

    //    // Cast a capsule from the player's position to check for collisions with the planet's surface
    //    float surfaceDistance = playerCollider.radius + 0.1f;
    //    if (Physics.CapsuleCast(playerPosition - surfaceDistance * surfaceNormal, playerPosition + surfaceDistance * surfaceNormal, playerCollider.radius, -surfaceNormal, out RaycastHit capsuleHit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        // Check if the intersection point is above the surface of the planet
    //        float planetRadius = (playerPosition - capsuleHit.point).magnitude;
    //        float offset = 0.2f;
    //        if (capsuleHit.point.magnitude > planetRadius + offset)
    //        {
    //            // Calculate the position of the intersection point and move the player to it
    //            playerPosition = capsuleHit.point + surfaceDistance * surfaceNormal;
    //        }
    //        else
    //        {
    //            // The player is too close to the surface, so stop it from moving further
    //            playerRigidbody.velocity = Vector3.zero;
    //            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    //        }
    //    }
    //    else
    //    {
    //        // Move the player in the direction of the input
    //        float maxSpeed = 5f;
    //        playerPosition += moveDirection * maxSpeed * Time.fixedDeltaTime;
    //    }

    //    // Ensure that the player stays on the surface of the planet while moving
    //    playerRigidbody.MovePosition(playerPosition);
    //    playerRigidbody.MoveRotation(surfaceRotation);
    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}



    //FLIES THROUGH A LOT:
    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Get the player's current position and the planet's center position
    //    Vector3 playerPosition = playerModel.transform.position;
    //    Vector3 planetCenter = Vector3.zero;

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = (planetCenter - playerPosition).normalized;
    //    Vector3 gravityForce = gravity * playerRigidbody.mass * -gravityDirection;

    //    float maxSpeed = 3f;

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);

    //    // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
    //    Vector3 surfaceNormal = Vector3.zero;
    //    if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        surfaceNormal = hit.normal;
    //    }
    //    Quaternion surfaceRotation = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal);
    //    Vector3 moveDirection = surfaceRotation * new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

    //    // Cast a capsule from the player's position to check for collisions with the planet's surface
    //    float surfaceDistance = playerCollider.radius + 0.1f;
    //    if (Physics.Raycast(playerPosition + 0.1f * surfaceNormal, -surfaceNormal, out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        // Check if the intersection point is above the surface of the planet
    //        float planetRadius = (playerPosition - raycastHit.point).magnitude;
    //        float offset = 0.2f;
    //        if (raycastHit.point.magnitude > planetRadius + offset)
    //        {
    //            // Calculate the position of the intersection point and move the player to it
    //            playerPosition = raycastHit.point + surfaceDistance * surfaceNormal;
    //        }
    //    }
    //    else
    //    {
    //        // Move the player in the direction of the input
    //        playerPosition += moveDirection * maxSpeed * Time.fixedDeltaTime;
    //    }



    //    // Calculate the player's final position and rotation after movement
    //    Vector3 finalPosition = playerPosition + moveDirection * maxSpeed * Time.fixedDeltaTime;
    //    Quaternion finalRotation = surfaceRotation;

    //    // Use CCD to move the player's position and rotation
    //    playerRigidbody.MovePosition(finalPosition);
    //    playerRigidbody.MoveRotation(finalRotation);

    //    // Check if a collision occurred during movement
    //    if (playerRigidbody.collisionCount > 0)
    //    {
    //        // Handle collision with planet surface here
    //    }

    //    //Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}


    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Get the player's current position and the planet's center position
    //    Vector3 playerPosition = playerModel.transform.position;
    //    Vector3 planetCenter = Vector3.zero;

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = (planetCenter - playerPosition).normalized;
    //    Vector3 gravityForce = gravity * playerRigidbody.mass * -gravityDirection;

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);

    //    // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
    //    Vector3 surfaceNormal = Vector3.zero;
    //    if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        surfaceNormal = hit.normal;
    //    }
    //    Quaternion surfaceRotation = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal);
    //    Vector3 moveDirection = surfaceRotation * new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

    //    // Cast a capsule from the player's position to check for collisions with the planet's surface
    //    float surfaceDistance = playerCollider.radius + 0.1f;
    //    if (Physics.CapsuleCast(playerPosition - surfaceDistance * surfaceNormal, playerPosition + surfaceDistance * surfaceNormal, playerCollider.radius, -surfaceNormal, out RaycastHit capsuleHit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        // Calculate the position of the intersection point and move the player to it
    //        playerPosition = capsuleHit.point + surfaceDistance * surfaceNormal;
    //    }
    //    else
    //    {
    //        // Move the player in the direction of the input
    //        float maxSpeed = 5f;
    //        playerPosition += moveDirection * maxSpeed * Time.fixedDeltaTime;
    //    }

    //    // Ensure that the player stays on the surface of the planet while moving
    //    playerRigidbody.MovePosition(playerPosition);
    //    playerRigidbody.MoveRotation(surfaceRotation);

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}






    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Get the player's current position and the planet's center position
    //    Vector3 playerPosition = playerModel.transform.position;
    //    Vector3 planetCenter = Vector3.zero;

    //    // Calculate the distance between the player and the center of the planet
    //    float distanceToPlanetCenter = Vector3.Distance(playerPosition, planetCenter);

    //    // Calculate the normal vector of the planet's surface at the player's current position
    //    Vector3 surfaceNormal = Vector3.zero;
    //    if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, distanceToPlanetCenter, LayerMask.GetMask("Planet")))
    //    {
    //        surfaceNormal = hit.normal;
    //    }

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = (planetCenter - playerPosition).normalized;
    //    Vector3 gravityForce = gravity * playerRigidbody.mass * -gravityDirection;

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);


    //    // Check if the player is on the surface of the planet
    //    float surfaceThreshold = playerCollider.height / 2f + 0.1f;
    //    if (distanceToPlanetCenter < surfaceThreshold)
    //    {
    //        // Player is on the surface, prevent them from falling through
    //        playerRigidbody.MovePosition(playerPosition - distanceToPlanetCenter * surfaceNormal);
    //    }
    //    else
    //    {
    //        // Player is not on the surface, update their position
    //        playerRigidbody.MovePosition(playerPosition + playerRigidbody.velocity * Time.fixedDeltaTime);

    //        // Check if the player is close enough to the surface to prevent them from falling through
    //        float surfaceDistance = distanceToPlanetCenter - surfaceThreshold;
    //        if (surfaceDistance < 0f)
    //        {
    //            playerRigidbody.MovePosition(playerPosition - surfaceDistance * surfaceNormal);
    //        }
    //    }

    //    // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
    //    Vector3 moveDirection = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal) * new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

    //    // Ensure that the player stays on the surface of the planet while moving
    //    playerRigidbody.MoveRotation(Quaternion.LookRotation(-surfaceNormal));
    //    float maxSpeed = 5f;
    //    float moveSpeed = moveDirection.magnitude * maxSpeed;
    //    if (moveSpeed > 0f)
    //    {
    //        moveDirection = moveDirection.normalized;
    //        playerRigidbody.MovePosition(playerPosition + moveDirection * moveSpeed * Time.fixedDeltaTime);
    //    }

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}


    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Get the player's current position and the planet's center position
    //    Vector3 playerPosition = playerModel.transform.position;
    //    Vector3 planetCenter = Vector3.zero;

    //    // Calculate the distance between the player and the center of the planet
    //    float distanceToPlanetCenter = Vector3.Distance(playerPosition, planetCenter);

    //    // Calculate the normal vector of the planet's surface at the player's current position
    //    Vector3 surfaceNormal = Vector3.zero;
    //    if (Physics.Raycast(playerPosition, -playerPosition.normalized, out RaycastHit hit, distanceToPlanetCenter, LayerMask.GetMask("Planet")))
    //    {
    //        surfaceNormal = hit.normal;
    //    }

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = -playerPosition.normalized;
    //    Vector3 gravityForce = gravity * playerRigidbody.mass * gravityDirection;

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);

    //    // Check if the player is on the surface of the planet
    //    float surfaceThreshold = playerCollider.height / 2f + 0.1f;
    //    if (distanceToPlanetCenter < surfaceThreshold)
    //    {
    //        // Player is on the surface, prevent them from falling through
    //        playerRigidbody.MovePosition(playerPosition - distanceToPlanetCenter * surfaceNormal);
    //    }
    //    else
    //    {
    //        // Player is not on the surface, calculate the new position based on the gravitational force
    //        Vector3 newPosition = playerPosition + playerRigidbody.velocity * Time.fixedDeltaTime;
    //        float newDistanceToPlanetCenter = Vector3.Distance(newPosition, planetCenter);
    //        if (newDistanceToPlanetCenter > distanceToPlanetCenter)
    //        {
    //            // Player is moving away from the planet, teleport them to the surface at the correct position
    //            playerRigidbody.MovePosition(planetCenter + surfaceThreshold * playerPosition.normalized);
    //        }
    //        else
    //        {
    //            // Player is falling towards the planet, update their position
    //            playerRigidbody.MovePosition(newPosition);
    //        }
    //    }

    //    // Calculate the movement direction of the player based on the input direction and the curvature of the planet's surface
    //    Vector3 moveDirection = Quaternion.FromToRotation(playerModel.transform.up, surfaceNormal) * new Vector3(inputDirection.x, 0f, inputDirection.y);
    //    moveDirection = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

    //    // Ensure that the player stays on the surface of the planet while moving
    //    playerRigidbody.MoveRotation(Quaternion.LookRotation(-surfaceNormal));
    //    float maxSpeed = 5f;
    //    float moveSpeed = moveDirection.magnitude * maxSpeed;
    //    if (moveSpeed > 0f)
    //    {
    //        moveDirection = moveDirection.normalized;
    //        playerRigidbody.MovePosition(playerPosition + moveDirection * moveSpeed * Time.fixedDeltaTime);
    //    }

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}




    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //if (downward)
    //{
    //    // Apply custom gravity to yVelocity
    //    Vector3 gravityDirection = (playerModel.transform.position - Program.planetModel.transform.position).normalized;
    //    yVelocity += gravityDirection.y * (gravity / 20f);

    //    // Clamp the maximum velocity to prevent the player from falling too quickly
    //    yVelocity = Mathf.Clamp(yVelocity, -2f, 2f);

    //    // Update the player's y position based on yVelocity
    //    playerModel.transform.position += new Vector3(0, yVelocity, 0);
    //}
    //else
    //{
    //    // Reset the player's y velocity if they are not moving downwards
    //    yVelocity = 0f;
    //}
    //Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();
    //CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();
    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    // Get the player's CapsuleCollider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    // Cast a sphere from the bottom of the player's capsule collider towards the planet's center
    //    float sphereRadius = playerCollider.radius - 0.1f; // reduce the radius slightly to prevent immediate collision
    //    float distanceToSurface = 0f;
    //    if (Physics.SphereCast(playerModel.transform.position - playerCollider.height * 0.5f * playerModel.transform.up, sphereRadius, -playerModel.transform.up, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
    //    {
    //        distanceToSurface = hit.distance - sphereRadius;
    //    }

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityDirection = Vector3.Normalize(playerModel.transform.position);
    //    Vector3 gravityForce = -gravity * playerRigidbody.mass * gravityDirection;

    //    // Apply the gravitational force to the player only if the distance to the surface is greater than 2
    //    if (distanceToSurface > 2f)
    //    {
    //        // Apply the gravitational force to the player
    //        playerRigidbody.AddForce(gravityForce);
    //    }

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);

    //}





    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Normalize the input direction
    //    if (inputDirection != Vector2.zero)
    //    {
    //        inputDirection = inputDirection.normalized;
    //    }

    //    // Get the player's Rigidbody component
    //    Rigidbody playerRigidbody = playerModel.GetComponent<Rigidbody>();

    //    LayerMask planetLayerMask = LayerMask.GetMask("Planet");

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityForce = -gravity * playerRigidbody.mass * Vector3.Normalize(playerModel.transform.position);

    //    // Apply the gravitational force to the player
    //    playerRigidbody.AddForce(gravityForce);

    //    // Calculate the movement vector based on the input direction and the player's move speed
    //    Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;

    //    // Apply the movement force to the player
    //    playerRigidbody.AddForce(movement);

    //    // Teleport the player to the surface of the planet if they fall through
    //    RaycastHit hit;
    //    if (Physics.Raycast(playerModel.transform.position, -playerModel.transform.position.normalized, out hit, Mathf.Infinity, planetLayerMask))
    //    {
    //        float distanceToSurface = hit.distance;
    //        float playerRadius = playerModel.GetComponent<MeshCollider>().bounds.extents.magnitude;

    //        if (distanceToSurface < playerRadius)
    //        {
    //            Vector3 surfaceNormal = hit.normal;
    //            Vector3 surfacePoint = hit.point + surfaceNormal * playerRadius;

    //            playerModel.transform.position = surfacePoint;

    //            Quaternion newRotation = Quaternion.FromToRotation(playerModel.transform.up, -surfaceNormal) * playerModel.transform.rotation;
    //            playerModel.transform.rotation = newRotation;
    //        }
    //    }

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}


    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Normalize the input direction
    //    if (inputDirection != Vector2.zero)
    //    {
    //        inputDirection = inputDirection.normalized;
    //    }

    //    // Get the player's Capsule Collider component
    //    CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //    LayerMask planetLayerMask = LayerMask.GetMask("Planet");

    //    // Calculate the normal vector of the planet's surface at the player's position
    //    Vector3 surfaceNormal = (playerModel.transform.position - Program.planetModel.transform.position).normalized;

    //    // Calculate the gravitational force on the player
    //    Vector3 gravityForce = -gravity * (playerModel.transform.position - Program.planetModel.transform.position).normalized;

    //    // Project the gravitational force onto the surface normal to calculate the normal force
    //    float projectedGravityForceMagnitude = Vector3.Dot(gravityForce, surfaceNormal);
    //    float normalForceMagnitude = Mathf.Max(0f, projectedGravityForceMagnitude);
    //    Vector3 normalForce = normalForceMagnitude * surfaceNormal;

    //    // Calculate the tangential force by subtracting the normal force from the gravitational force
    //    Vector3 tangentialForce = gravityForce - normalForce;

    //    // Calculate the acceleration vector by dividing the net force by the player's mass
    //    Vector3 acceleration = (normalForce + tangentialForce) / mass;

    //    // Update the player's y velocity based on the acceleration
    //    yVelocity += acceleration.y * Time.deltaTime;

    //    // Clamp the maximum velocity to prevent the player from falling too quickly
    //    //yVelocity = Mathf.Clamp(yVelocity, -2f, 2f);

    //    // Update the player's y position based on yVelocity
    //    playerModel.transform.position += new Vector3(0, yVelocity, 0);

    //    // Cast a ray towards the center of the planet
    //    Vector3 center = (Program.planetModel.transform.position - playerModel.transform.position).normalized;
    //    RaycastHit hit;
    //    if (Physics.Raycast(playerModel.transform.position, center, out hit, Mathf.Infinity, planetLayerMask))
    //    {
    //        // Check if the hit point is inside the planet's surface
    //        float distanceToHit = Vector3.Distance(playerModel.transform.position, hit.point);
    //        if (distanceToHit < playerCollider.radius)
    //        {
    //            Vector3 displacement = playerModel.transform.position - Program.planetModel.transform.position;
    //            float distanceFromSurface = displacement.magnitude - (Program.planetModel.GetComponent<SphereCollider>().radius - 0.1f);
    //            Vector3 surfacePosition = playerModel.transform.position - surfaceNormal * distanceFromSurface;
    //            playerModel.transform.position = surfacePosition + surfaceNormal * (playerCollider.radius + 0.1f); // add a small offset to prevent immediate collision


    //        }
    //    }

    //    // Check if the player is moving downwards
    //    if (downward)
    //    {
    //        // Apply custom gravity to yVelocity
    //        Vector3 gravityDirection = (playerModel.transform.position - Program.planetModel.transform.position).normalized;
    //        yVelocity += gravityDirection.y * (gravity / 20f);

    //        // Clamp the maximum velocity to prevent the player from falling too quickly
    //        yVelocity = Mathf.Clamp(yVelocity, -2f, 2f);

    //        // Update the player's y position based on yVelocity
    //        playerModel.transform.position += new Vector3(0, yVelocity, 0);
    //    }
    //    else
    //    {
    //        // Reset the player's y velocity if they are not moving downwards
    //        yVelocity = 0f;
    //    }

    //    // Calculate the movement vector based on the input direction and the player's move speed
    //    Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;

    //    // Calculate the player's position after moving
    //    Vector3 newPosition = playerModel.transform.position + movement;

    //    // Cast a ray towards the center of the planet
    //    Vector3 centerDirection = (Program.planetModel.transform.position - newPosition).normalized;
    //    if (Physics.Raycast(newPosition, centerDirection, out RaycastHit hit2, Mathf.Infinity, planetLayerMask))
    //    {
    //        // Check if the hit point is inside the planet's surface
    //        float distanceToHit = Vector3.Distance(playerModel.transform.position, hit2.point);
    //        if (distanceToHit < playerCollider.radius)
    //        {
    //            // Move the player outside the planet's surface
    //            Vector3 displacement = newPosition - Program.planetModel.transform.position;
    //            playerModel.transform.position = Program.planetModel.transform.position + displacement.normalized * (Program.planetModel.GetComponent<SphereCollider>().radius + playerCollider.radius + 0.1f);
    //        }
    //    }

    //    // Update the player's position
    //    playerModel.transform.position = newPosition;

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}

    // Check if the player is on the surface of the planet
    //float surfaceDistance = Program.planetModel.GetComponent<SphereCollider>().radius + 0.1f;
    //bool onSurface = false;
    //Collider[] hits = Physics.OverlapCapsule(
    //    playerCollider.bounds.center - Vector3.up * playerCollider.bounds.extents.y,
    //    playerCollider.bounds.center + Vector3.up * playerCollider.bounds.extents.y,
    //    playerCollider.radius,
    //    planetLayerMask
    //);
    //if (hits.Length > 0 && Vector3.Distance(playerModel.transform.position, hits[0].ClosestPoint(playerModel.transform.position)) <= surfaceDistance)
    //{
    //    onSurface = true;
    //}

    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //// Normalize the input direction
    //if (inputDirection != Vector2.zero)
    //{
    //    inputDirection = inputDirection.normalized;
    //}

    //// Get the player's Capsule Collider component
    //CapsuleCollider playerCollider = playerModel.GetComponent<CapsuleCollider>();

    //// Define the position and height of the capsule for overlap testing
    //Vector3 capsuleCenter = position + playerCollider.center;
    //float capsuleHeight = playerCollider.height - playerCollider.radius * 2f;

    //// Check if the player is colliding with the planet's collider
    //bool onSurface = false;
    //Collider[] hits = Physics.OverlapCapsule(
    //    capsuleCenter - Vector3.up * capsuleHeight / 2f,
    //    capsuleCenter + Vector3.up * capsuleHeight / 2f,
    //    playerCollider.radius
    //);
    //    foreach (Collider onehit in hits)
    //    {
    //        if (onehit.gameObject == Program.planetModel)
    //        {
    //            onSurface = true;

    //            // Move the player outside the planet's collider if they are inside it
    //            Vector3 closestPointOnSurface = onehit.ClosestPoint(position);
    //            Vector3 displacement = position - closestPointOnSurface;
    //            position += displacement.normalized * (displacement.magnitude + 0.1f);

    //            break;
    //        }
    //    }

    //    // Check if the player has entered the planet's surface
    //    Collider planetCollider = Program.planetModel.GetComponent<Collider>();
    //    Vector3 closestPointOnPlanet = planetCollider.ClosestPoint(position);
    //    if (closestPointOnPlanet == position)
    //    {
    //        // Move the player outside the planet's surface
    //        Vector3 displacement = position - Program.planetModel.transform.position;
    //        position = Program.planetModel.transform.position + displacement.normalized * (planetCollider.bounds.extents.magnitude + 0.1f);
    //    }

    //    // Calculate the movement vector based on the input direction and the player's move speed
    //    Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;

    //// Calculate the distance from the player's current position to the planet's center
    //float distanceToCenter = Vector3.Distance(position, Program.planetModel.transform.position);

    //    if (onSurface)
    //    {
    //        // Calculate the normal vector of the planet's surface at the player's position
    //        Vector3 surfaceNormal = (position - Program.planetModel.transform.position).normalized;

    //        // Calculate the gravitational force on the player
    //        Vector3 gravityForce = -gravity * (position - Program.planetModel.transform.position).normalized;

    //        // Project the gravitational force onto the surface normal to calculate the normal force
    //        float normalForceMagnitude = Mathf.Max(0f, projectedGravityForceMagnitude);
    //        Vector3 normalForce = normalForceMagnitude * surfaceNormal;

    //        // Calculate the tangential force by subtracting the normal force from the gravitational force
    //        Vector3 tangentialForce = gravityForce - normalForce;

    //        // Calculate the acceleration vector by dividing the net force by the player's mass
    //        Vector3 acceleration = (normalForce + tangentialForce) / mass;

    //        // Update the player's y velocity based on the acceleration
    //        yVelocity += acceleration.y * Time.deltaTime;

    //        // Clamp the maximum velocity to prevent the player from falling too quickly
    //        yVelocity = Mathf.Clamp(yVelocity, -2f, 2f);

    //        // Update the player's y position based on yVelocity
    //        position += new Vector3(0, yVelocity, 0);

    //        // Check if the player has entered the planet's surface
    //        if (distanceToCenter <= Program.planetModel.GetComponent<SphereCollider>().radius)
    //        {
    //            // Move the player outside the planet's surface
    //            Vector3 displacement = position - Program.planetModel.transform.position;
    //            position = Program.planetModel.transform.position + displacement.normalized * (Program.planetModel.GetComponent<SphereCollider>().radius + 0.1f);
    //        }
    //    }

    //    else
    //    {
    //        // Apply custom gravity to yVelocity
    //        Vector3 gravityDirection = (position - Program.planetModel.transform.position).normalized;
    //        yVelocity += gravityDirection.y * (gravity / 20f);

    //        // Clamp the maximum velocity to prevent the player from falling too quickly
    //        yVelocity = Mathf.Clamp(yVelocity, -2f, 2f);

    //        // Update the player's y position based on yVelocity
    //        position += new Vector3(0, yVelocity, 0);
    //    }
    //    // Check if the player is about to collide with the planet's surface
    //    RaycastHit hit;
    //    if (Physics.Raycast(position, movement.normalized, out hit, movement.magnitude))
    //    {
    //        // If the raycast hits something and it's not a trigger collider
    //        if (!hit.collider.isTrigger)
    //        {
    //            // Set the player's position to the point where the raycast hit the surface
    //            position = hit.point;

    //            // Adjust the movement vector to be tangent to the surface of the planet
    //            Vector3 surfaceNormal = hit.normal;
    //            Vector3 tangent = Vector3.Cross(surfaceNormal, Vector3.Cross(movement.normalized, surfaceNormal)).normalized;
    //            movement = tangent * movement.magnitude;

    //            // Reset the player's y velocity
    //            yVelocity = 0;

    //            // Check if the player has entered the planet's surface
    //            if (distanceToCenter <= Program.planetModel.GetComponent<SphereCollider>().radius)
    //            {
    //                // Move the player outside the planet's surface
    //                Vector3 displacement = position - Program.planetModel.transform.position;
    //                position = Program.planetModel.transform.position + displacement.normalized * (Program.planetModel.GetComponent<SphereCollider>().radius + 0.1f);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Update the player's position
    //        position += movement;
    //    }

    //    // Set yVelocity to 0 if the player is not moving downwards
    //    if (!downward)
    //    {
    //        yVelocity = 0;
    //    }

    //    // Move the playerModel object to the updated position
    //    playerModel.transform.position = position;

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}


    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    // Normalize the input direction
    //    if (inputDirection != Vector2.zero)
    //    {
    //        //inputDirection = Vector2.Normalize(inputDirection);
    //        inputDirection = inputDirection.normalized;
    //    }

    //    // Calculate the movement vector based on the input direction and the player's move speed
    //    Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;

    //    // Update the player's position
    //    position += movement;

    //    if (!downward)
    //    {
    //        yVelocity = 0;
    //        if (inputs[6])
    //        {
    //            animation = "jump";
    //            //yVelocity = 5;
    //        }
    //    }


    //    //Apply custom gravity to yVelocity
    //    Vector3 gravityDirection = (position - new Vector3(0, 0, 0)).normalized;
    //    yVelocity += gravityDirection.y * gravity;

    //    //Update the player's y position based on yVelocity
    //    position += new Vector3(0, yVelocity, 0);

    //    //If the player's y position is below the ground level
    //    if (position.y < 0)
    //    {
    //        // Set the player's y position to the ground level
    //        position = new Vector3(position.x, 0, position.z);
    //    }
    //    //Dispatcher.RunOnMainThread(() =>
    //    //{
    //        playerModel.transform.position = position;
    //    //});

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID, playerModel);
    //}


    //Apply custom gravity to yVelocity
    //Vector3 gravityDirection = (position - new Vector3(0, 0, 0)).normalized;
    //yVelocity += gravityDirection.y * gravity;
    //position = new Vector3(0, yVelocity, 0);

    // yVelocity += gravity;

    //Apply custom gravity force towards planet center at(0,0,0)
    //Vector3 gravityCenter = new Vector3(0, 0, 0);
    //float gravityStrength = 9.8f;

    //Vector3 gravityDirection = (gravityCenter - transform.position).normalized;

    //float fixedTimeStep = 1.0f / 30.0f; // Fixed time step of 1/30 seconds
    //position += gravityDirection * gravityStrength * fixedTimeStep;

    //public void Move(Vector2 inputDirection, bool downward, int connectionID)
    //{
    //    Debug.Log("Move called");
    //    // Calculate the movement vector based on the input direction and the player's move speed
    //    Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;
    //    Debug.Log("Move calleda");
    //    // Calculate the gravity force vector based on the player's mass and the planet's mass
    //    //Vector3 gravityForce = CalculateGravity();
    //    //Vector3 gravityForce = new Vector3(0, -9.8f, 0);
    //    Debug.Log("Move calledb");
    //    // Update the player's position based on the movement vector and the gravity force vector
    //    //position += movement + gravityForce;
    //    Debug.Log("Move calledc");
    //    // If the player is not pressing the jump button
    //    if (!downward)
    //    {
    //        // Reset the y velocity
    //        yVelocity = 0;

    //        // If the player is pressing the jump button
    //        if (inputs[6])
    //        {
    //            // Set the animation to "jump"
    //            animation = "jump";

    //            // Increase the y velocity to push against gravity
    //            yVelocity = 5;
    //        }
    //    }
    //    Debug.Log("Move calledd");
    //    // Update the y velocity based on gravity
    //    yVelocity += gravity;
    //    Debug.Log("Move callede");
    //    // Update the player's y position based on yVelocity
    //    position += new Vector3(0, yVelocity, 0);
    //    Debug.Log("Move calledf");
    //    //playerModel.transform.position = position;
    //    //Debug.Log(position);

    //    Dispatcher.RunOnMainThread(() =>
    //    {
    //        Debug.Log("HI");
    //        playerModel.transform.position = position;
    //        Debug.Log("Bye");
    //    });
    //    Debug.Log("passed");

    //    // Send the player's updated position to the client
    //    NetworkSend.PlayerPosition(connectionID);
    //    Debug.Log("Send updated player position: " + position);
    //}

    //private Vector3 CalculateGravity()
    //{
    //    Debug.Log("CalulateGravity Called");
    //        Vector3 planetCenter = Program.planetCenter;
    //        Debug.Log("CalulateGravity Calleda");
    //        // Calculate the distance between the player and the center of the planet
    //        float distance = Vector3.Distance(position, planetCenter);
    //        Debug.Log("CalulateGravity Calledb");
    //        // Calculate the gravity force based on the player's mass and the planet's mass
    //        float gravityForce = Constants.GRAVITATIONAL_CONSTANT * (mass * Program.planetMass) / (distance * distance);
    //        Debug.Log("CalulateGravity Calledc");
    //        // Calculate the gravity direction based on the player's position and the center of the planet
    //        Vector3 gravityDirection = (planetCenter - position).normalized;
    //        Debug.Log("CalulateGravity Calledd");
    //        // Return the gravity force vector
    //        return gravityDirection * gravityForce;
    //}

    public void UpdateMovement(int connectionID, bool[] _inputs)
    {
        Debug.Log("Update Movement called");
        inputs = _inputs;
        //rotation = _rotation;
        MovementUpdate(connectionID);
    }

    public void UpdateRotation(float[] mouseInputs)
    {
        // Default values for mouse sensitivity
        const float DEFAULT_MOUSE_SENSITIVITY = 30.0f;

        // Calculate the pitch and yaw based on the mouse inputs
        float pitch = -mouseInputs[1] * DEFAULT_MOUSE_SENSITIVITY;
        float yaw = mouseInputs[0] * DEFAULT_MOUSE_SENSITIVITY;

        // Convert pitch and yaw from degrees to radians
        pitch = pitch * (float)(Math.PI / 180);
        yaw = yaw * (float)(Math.PI / 180);

        // Calculate the direction the player is currently looking based on the current pitch and yaw
        Vector3 currentDirection = new Vector3(
            Mathf.Sin(currentYaw) * Mathf.Cos(currentPitch),
            Mathf.Sin(currentPitch),
            Mathf.Cos(currentYaw) * Mathf.Cos(currentPitch)
        );

        // Update the current pitch and yaw with the latest pitch and yaw inputs
        currentPitch += pitch;
        currentYaw += yaw;

        // Calculate the new target direction based on the updated pitch and yaw
        Vector3 targetDirection = new Vector3(
            Mathf.Sin(currentYaw) * Mathf.Cos(currentPitch),
            Mathf.Sin(currentPitch),
            Mathf.Cos(currentYaw) * Mathf.Cos(currentPitch)
        );

        // Calculate the rotation required to rotate the current direction to the target direction
        Quaternion targetRotation = Quaternion.FromToRotation(currentDirection, targetDirection);

        // Update the player's rotation
        rotation = targetRotation * rotation;
        Dispatcher.RunOnMainThread(() =>
        {
            FinalRotation(connectionID, rotation);
            //playerModel.transform.rotation = rotation;
        });

        //NetworkSend.PlayerRotation(connectionID, rotation);
        //Debug.Log("Send updated player rotation: " + rotation);
    }

    public void FinalRotation(int connectionID, Quaternion rotation)
    {
        playerModel.transform.rotation = rotation;
        NetworkSend.PlayerRotation(connectionID);
        Debug.Log("Send updated player rotation: " + rotation);
    }
   
}
