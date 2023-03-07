//using UnityEngine;
using System;
using System.Numerics;

namespace NewServer
{
    public class Player
    {
        public int connectionID;
        public string username;
        public bool inGame;
        public string Token;

        public string animation;

        public Vector3 position;
        public Quaternion rotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        private float yVelocity = 0;
        public float gravity = -9.81f;

        public float currentYaw;
        public float currentPitch;

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            connectionID = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;
            currentYaw = 0;
            currentPitch = 0;

            inputs = new bool[4];

        }


        public void Update(int connectionID)
        {
            // Initialize _inputDirection to zero
            Vector2 _inputDirection = Vector2.Zero;

            // Update the animation based on the player's inputs
            animation = "idle";
            if (inputs[0])
            {
                animation = "walk";
                _inputDirection.Y += 1;
                if (inputs[5])
                {
                    moveSpeed = 10f / Constants.TICKS_PER_SEC;
                    animation = "run";
                }
            }
            if (inputs[1])
            {
                animation = "walk";
                _inputDirection.Y -= 1;
                if (inputs[5])
                {
                    moveSpeed = 10f / Constants.TICKS_PER_SEC;
                    animation = "run";
                }
            }
             if (inputs[2])
            {
                animation = "walk";
                _inputDirection.X += 1;
                if (inputs[5])
                {
                    moveSpeed = 10f / Constants.TICKS_PER_SEC;
                    animation = "run";
                }
            }
             if (inputs[3])
            {
                animation = "walk";
                _inputDirection.X -= 1;
                if (inputs[5])
                {
                    moveSpeed = 10f / Constants.TICKS_PER_SEC;
                    animation = "run";
                }
            }

            // Set downward to true if the player is pressing the jump button
            bool downward = inputs[4];

            // Call the Move method with the current input direction and the jump flag
            Move(_inputDirection, downward, connectionID);
        }


        public void Move(Vector2 inputDirection, bool downward, int connectionID)
        {
            // Normalize the input direction
            if (inputDirection != Vector2.Zero)
            {
                inputDirection = Vector2.Normalize(inputDirection);
                //inputDirection = inputDirection.normalize;
            }

            // Calculate the movement vector based on the input direction and the player's move speed
            Vector3 movement = new Vector3(inputDirection.X, 0, inputDirection.Y) * moveSpeed;

            // Update the player's position
            position += movement;

            if (!downward)
            {
                yVelocity = 0;
                if (inputs[6])
                {
                    animation = "jump";
                    yVelocity = 5;
                }
            }
            yVelocity += gravity;

            // Update the player's y position based on yVelocity
            position += new Vector3(0, yVelocity, 0);

            // If the player's y position is below the ground level
            if (position.Y < 0)
            {
                // Set the player's y position to the ground level
                position = new Vector3(position.X, 0, position.Z);
            }

        // Send the player's updated position to the client
        NetworkSend.PlayerPosition(connectionID, position);
        }

        public void UpdateMovement(int connectionID, bool[] _inputs)
            {
                inputs = _inputs;
                //rotation = _rotation;
                Update(connectionID);
            }

        public void UpdateRotation(float[] mouseInputs)
        {
            float mouseX = mouseInputs[0];
            float mouseY = mouseInputs[1];

            // Default values for mouse sensitivity
            const float DEFAULT_MOUSE_SENSITIVITY = 1.0f;

            // Calculate the pitch and yaw based on the mouse inputs
            float pitch = -mouseY * DEFAULT_MOUSE_SENSITIVITY;
            float yaw = mouseX * DEFAULT_MOUSE_SENSITIVITY;

            // Convert pitch and yaw from degrees to radians
            pitch = pitch * (float)(Math.PI / 180);
            yaw = yaw * (float)(Math.PI / 180);

            currentPitch += pitch;
            currentYaw += yaw;


            // Create a quaternion for the pitch and yaw
            Quaternion pitchRotation = Quaternion.CreateFromYawPitchRoll(currentYaw, currentPitch, 0);

            NetworkSend.PlayerRotation(connectionID, pitchRotation);
        }


        //Slow rotation bring back to center
        //public void UpdateRotation(float[] mouseInputs)
        //{
        //    // Calculate the new rotation based on mouse input
        //    Quaternion targetRotation = Quaternion.CreateFromYawPitchRoll(mouseInputs[1], mouseInputs[0], 0);

        //    // Clamp the player's vertical rotation to avoid flipping over
        //    targetRotation = ClampRotation(targetRotation);

        //    // Smoothly interpolate towards the target rotation
        //    float rotationSpeed = 5f; // adjust as needed
        //    rotation = Quaternion.Slerp(rotation, targetRotation, rotationSpeed * (1 / 60f));
        //    NetworkSend.PlayerRotation(connectionID, rotation);
        //}

        //private Quaternion ClampRotation(Quaternion rot)
        //{
        //    var angleX = 2.0f * (float)Math.Atan(rot.X / rot.W);
        //    float maxVerticalRotation = 85f; // adjust as needed

        //    angleX = (float)Math.Max(-maxVerticalRotation, Math.Min(maxVerticalRotation, angleX));

        //    rot.X = (float)Math.Tan(0.5f * angleX);

        //    return rot;
        //}








        //public void UpdateRotation(float[] mouseInputs)
        //{
        //    float sensitivity = 0.01f;
        //    float mouseX = mouseInputs[0];
        //    float mouseY = mouseInputs[1];
        //    // Perform some calculations to update the player's orientation based on the mouse inputs
        //    // You can use the mouseX and mouseY values to determine the new orientation
        //    // Some pseudocode to illustrate one way you could do this:
        //    // Create a Quaternion variable to store the new orientation
        //    Quaternion newOrientation = rotation;
        //    // Calculate the new orientation based on mouseX and mouseY
        //    // You may need to adjust the sensitivity of the mouse inputs
        //    newOrientation.x = mouseX * sensitivity;
        //    newOrientation.y = mouseY * sensitivity;
        //    // Update the player's orientation
        //    rotation = newOrientation;
        //    // Send the updated orientation to all other players
        //    NetworkSend.PlayerRotation(connectionID, rotation);
        //}




        /// <summary>Processes player input and moves the player.</summary>
        //public void Update(int connectionID)
        //{
        //    Vector2 _inputDirection = Vector2.Zero;

        //        if (inputs[0])
        //        {
        //            animation = "walk";
        //            _inputDirection.Y += 1;
        //        if (inputs[5])
        //        {
        //            moveSpeed = 10f / Constants.TICKS_PER_SEC;
        //            animation = "run";
        //        }
        //    }
        //    else if (inputs[1])
        //        {
        //            animation = "walk";
        //            _inputDirection.Y -= 1;
        //        if (inputs[5])
        //        {
        //            moveSpeed = 10f / Constants.TICKS_PER_SEC;
        //            animation = "run";
        //        }
        //    }
        //    else if (inputs[2])
        //        {
        //            animation = "walk";
        //            _inputDirection.X += 1;
        //        if (inputs[5])
        //        {
        //            moveSpeed = 10f / Constants.TICKS_PER_SEC;
        //            animation = "run";
        //        }
        //    }
        //    else if (inputs[3])
        //        {
        //            animation = "walk";
        //            _inputDirection.X -= 1;
        //        if (inputs[5])
        //        {
        //            moveSpeed = 10f / Constants.TICKS_PER_SEC;
        //            animation = "run";
        //        }
        //    }
        //    else
        //    {
        //        animation = "idle";
        //    }

        //    if (inputs[4])
        //    {
        //        downward = true;
        //    }



        //    Move(_inputDirection, downward, connectionID);
        //}

        /// <summary>Calculates the player's desired movement direction and moves him.</summary>
        /// <param name="_inputDirection"></param>
        //private void Move(Vector2 _inputDirection, bool downward, int connectionID)
        //{
        //    Console.WriteLine("Move is called!");
        //    Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);

        //    Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

        //    Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;

        //    position += _moveDirection * moveSpeed;

        //    //if (downward)
        //    //{
        //    //    _moveDirection.Y -= (float)9.8;
        //    //} else 
        //    if (!downward)
        //    {
        //        yVelocity = 0;
        //        if (inputs[6])
        //        {
        //            animation = "jump";
        //            yVelocity = 5;
        //        }
        //    }
        //    yVelocity += gravity;

        //    _moveDirection.Y = yVelocity;

        //    NetworkSend.PlayerPosition(connectionID, this);
        //    //NetworkSend.PlayerRotation(connectionID, this);
        //}

        /// <summary>Updates the player input with newly received input.</summary>
        /// <param name="_inputs">The new key inputs.</param>
        /// <param name="_rotation">The new rotation.</param>

    }
}
