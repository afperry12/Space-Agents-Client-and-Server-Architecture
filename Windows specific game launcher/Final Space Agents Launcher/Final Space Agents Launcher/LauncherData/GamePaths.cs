using System;
using System.IO;

namespace Final_Space_Agents_Launcher.LauncherData
{

    public class GamePaths
    {
        public string RootPath;
        public string GamesDirectory;
        public string GameVersionFile;
        public string ExecutableFile;

        public GamePaths(string Version)
        {
            Console.WriteLine("GamesPathpre");
            RootPath = Directory.GetCurrentDirectory();
            GamesDirectory = Path.Combine(RootPath, "Versions");
            Console.WriteLine("GamesPath1: "+GamesDirectory);
            GameVersionFile = Path.Combine(RootPath);
            Console.WriteLine("GamesPath2: " + GameVersionFile);
            ExecutableFile = Path.Combine(RootPath, "SpaceAgents.exe");
            Console.WriteLine("GamesPath3: " + ExecutableFile);

            if (!Directory.Exists(GamesDirectory))
            {
                Directory.CreateDirectory(GamesDirectory);
            }

            if (!Directory.Exists(GameVersionFile))
            {
                Directory.CreateDirectory(GameVersionFile);
            }



        }



    }

}
