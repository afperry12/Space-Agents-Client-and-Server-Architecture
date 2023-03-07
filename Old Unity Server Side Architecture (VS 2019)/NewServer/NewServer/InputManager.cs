//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Numerics;

//namespace NewServer
//{
//    public class InputManager
//    {

//        public enum Keys
//        {
//            None,
//            W,
//            A,
//            S,
//            D
//        }

//        //public static void TryToMove(int connectionID, Keys wKey, Keys aKey, Keys sKey, Keys dKey)
//        public static void TryToMove(int connectionID, Keys key)
//        {
//            Vector3 tempPosition = GameManager.playerList[connectionID].position;

//            //if (wKey == Keys.None && aKey == Keys.None && sKey == Keys.None && dKey ==Keys.None) return;
//            if (key == Keys.None) return;

//            Player player = GameManager.playerList[connectionID];

//            //if (wKey == Keys.W)
//            //{
//            //    tempPosition.X += GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            //    tempPosition.Z += GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            //}
//            //else if (aKey == Keys.A)
//            //{
//            //    tempPosition.X -= GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            //    tempPosition.Z += GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            //}
//            //else if (sKey == Keys.S)
//            //{
//            //    tempPosition.X -= GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            //    tempPosition.Z -= GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            //}
//            //else if (dKey == Keys.D)
//            //{
//            //    tempPosition.X += GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            //    tempPosition.Z -= GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            //}            

//            if (key == Keys.W)
//            {
//                tempPosition.X += GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//                tempPosition.Z += GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            }
//            else if (key == Keys.A)
//            {
//                tempPosition.X -= GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//                tempPosition.Z += GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            }
//            else if (key == Keys.S)
//            {
//                tempPosition.X -= GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//                tempPosition.Z -= GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//            }
//            else if (key == Keys.D)
//            {
//                tempPosition.X += GameManager.playerSpeed * ConvertRotationCos(player.rotation);
//                tempPosition.Z -= GameManager.playerSpeed * ConvertRotationSin(player.rotation);
//            }

//            GameManager.playerList[connectionID].position = tempPosition;

//            NetworkSend.SendPlayerMove(connectionID, GameManager.playerList[connectionID].position.X, GameManager.playerList[connectionID].position.Y, GameManager.playerList[connectionID].position.Z);
//        }

//        public static float ConvertRotationSin(float rotation)
//        {
//            return (float)Math.Round(Math.Sin(rotation * (Math.PI / 180)), 4);
//        }

//        public static float ConvertRotationCos(float rotation)
//        {
//            return (float)Math.Round(Math.Cos(rotation * (Math.PI / 180)), 4);
//        }
//    }
//}
