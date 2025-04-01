using System;
using System.Media;
using System.IO; // FullPaTH

namespace PROG_ASSIGN1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ===== TEST CODE =====
            string path = "welcome.wav";
            Console.WriteLine($"File exists: {File.Exists(path)}");
            Console.WriteLine($"Full path: {Path.GetFullPath(path)}");  //I JUST WANTED TO TEST IF MY WAV FILE IS VALID OR NOT BEAUSE OF THE WAY I RECORDED IT
            // =====================

            // Play sound if file exists, IF NOT I WILL GET AN ERROR
            if (File.Exists(path))
            {
                PlayWelcomeSound();
                Console.WriteLine("Playing welcome sound...");
            }
            else
            {
                Console.WriteLine("Error: welcome.wav not found in output directory!");
            }

            // Rest of my program
            ShowAsciiArt();
            Console.WriteLine("\nProgram is running smoothly!"); //interaction and dictionary must occupy this part
            Console.WriteLine("Press any key to exit..."); // this messege i want when the program is finishing running
            Console.ReadKey();
        }

        private static void PlayWelcomeSound()
        {
            try
            {
                using (var player = new SoundPlayer("welcome.wav"))
                {
                    player.PlaySync(); // This gonna wait until sound finishes
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio error: {ex.Message}");
            }
        }

        private static void ShowAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  
      ____  ____  _____ _   _ ____ _____ _   _ _____ ____  
     / ___||  _ \| ____| | | / ___|_   _| | | | ____|  _ \ 
     \___ \| |_) |  _| | | | \___ \ | | | |_| |  _| | |_) |
      ___) |  __/| |___| |_| |___) || | |  _  | |___|  __/     //Welcome design, ascii art
     |____/|_|   |_____|\___/|____/ |_| |_| |_|_____|_|    
            ");
            Console.ResetColor();
        }
    }
}