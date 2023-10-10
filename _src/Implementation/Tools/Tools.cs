using System;
using System.IO;

namespace Implementation.Tools
{
    public static class Tools
    {
        public static bool IsPrime(int number)
        {
            switch (number)
            {
                case <= 1:
                    return false;
                case 2:
                    return true;
            }
            
            if (number % 2 == 0)
            {
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
                
            return true;        
        }

        public static void CreateFileAndDirectory(string path)
        {
            var directory = Path.GetDirectoryName(path) ?? "";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(path)) return;
            
            File.Create(path).Close();
        }
        
        public static void CreateFileAndDirectory(string path, string defaultContent)
        {
            CreateFileAndDirectory(path);
            File.WriteAllText(path, defaultContent);
        }
    }
}
