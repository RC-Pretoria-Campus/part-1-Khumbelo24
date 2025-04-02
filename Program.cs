using System;
using System.Collections.Generic;
using System.Threading;
using System.Speech.Synthesis;

namespace PROG_ASSIGN1
{
    internal class Program
    {
        static string userName = "";
        static Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // General chatbot information
            { "how are you", "I'm functioning perfectly! Thanks for asking." },
            { "what's your purpose", "I'm here to help you understand cybersecurity concepts and stay safe online." },
            { "what can I ask you about", "You can ask me about password safety, phishing awareness, safe browsing, and general cybersecurity tips!" },
            
            // Password safety
            { "password", "Strong passwords are at least 12 characters long with a mix of uppercase, lowercase, numbers, and symbols. Avoid using personal information or common words." },
            { "how often should i change passwords", "It's recommended to change passwords every 3-6 months, and immediately if you suspect a breach." },
            { "password manager", "Password managers securely store all your passwords so you only need to remember one master password. They can also generate strong unique passwords for each site." },
            
            // Phishing awareness
            { "phishing", "Phishing is when attackers pose as trusted entities to trick you into revealing sensitive information like passwords or credit card details." },
            { "identify phishing", "Look for suspicious sender emails, spelling/grammar errors, urgent requests, unusual links, and requests for personal information." },
            { "suspect phishing", "Don't click any links or download attachments. Report the email to your IT department or forward to your email provider's phishing report address." },
            
            // Safe browsing
            { "browse safely", "Always use updated browsers, look for HTTPS in the URL, be cautious with downloads, and consider using a VPN when on public Wi-Fi." },
            { "https", "HTTPS (Hypertext Transfer Protocol Secure) encrypts data between your browser and the website, protecting your information from eavesdroppers." },
            { "vpn", "A VPN (Virtual Private Network) encrypts your internet connection, protecting your privacy and allowing you to browse more securely, especially on public networks." }
        };

        static void Main(string[] args)
        {
            // Play welcome speech using Speech Synthesis
            PlayWelcomeSpeech();

            // Display ASCII art
            ShowAsciiArt();

            // Greeting and get user name
            DisplayColoredMessage("\n╭─────────────────────────────────────────────────╮", ConsoleColor.Blue);
            DisplayColoredMessage("│   Welcome to the Cybersecurity Awareness Bot!   │", ConsoleColor.Cyan);
            DisplayColoredMessage("╰─────────────────────────────────────────────────╯", ConsoleColor.Blue);

            GetUserName();

            DisplayColoredMessage($"\nHello {userName}! I'm here to help you stay safe online.", ConsoleColor.Green);
            TypeWriterEffect("You can ask me questions about password safety, phishing, and safe browsing.", 30, ConsoleColor.Yellow);
            TypeWriterEffect("Type 'exit' when you're done.", 30, ConsoleColor.Yellow);

            // Main interaction loop
            bool continueChat = true;
            while (continueChat)
            {
                Console.WriteLine();
                DisplayColoredMessage("╭─────────────────────────────────────────────────╮", ConsoleColor.Blue);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("│ Your question: ");
                string userInput = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();
                DisplayColoredMessage("╰─────────────────────────────────────────────────╯", ConsoleColor.Blue);

                // Process user input
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    DisplayColoredMessage("Please type a question. I'm here to help!", ConsoleColor.Yellow);
                    continue;
                }

                if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit" || userInput.ToLower() == "bye")
                {
                    continueChat = false;
                    DisplayColoredMessage($"\nThank you for chatting with me, {userName}! Stay safe online!", ConsoleColor.Cyan);
                    continue;
                }

                // Get and display response
                string response = GetResponse(userInput);
                DisplayFormattedResponse(response);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void PlayWelcomeSpeech()
        {
            try
            {
                DisplayColoredMessage("Initializing voice greeting...", ConsoleColor.Gray);

                // Create a new SpeechSynthesizer instance
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {
                    // Configure the voice (optional)
                    synthesizer.Rate = 0; // Normal speed
                    synthesizer.Volume = 100; // Full volume

                    // Set the welcome message
                    string welcomeMessage = "Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.";

                    // Speak the welcome message
                    synthesizer.Speak(welcomeMessage);

                    DisplayColoredMessage("Voice greeting complete.", ConsoleColor.Gray);
                }
            }
            catch (Exception ex)
            {
                DisplayColoredMessage($"Speech synthesis error: {ex.Message}", ConsoleColor.Red);
                DisplayColoredMessage("Continuing without voice greeting.", ConsoleColor.Yellow);
            }
        }

        private static void ShowAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  
      ____  ____  _____ *   * ____ _____ *   * _____ ____  
     / ___||  * \| *___| | | / ___|_   *| | | | *___|  _ \ 
     \___ \| |_) |  *| | | | \*__ \ | | | |_| |  *| | |*) |
      ___) |  **/| |**_| |_| |___) || | |  *  | |*__|  __/     
     |____/|_|   |_____|\___/|____/ |_| |_| |_|_____|_|    
            ");
            Console.WriteLine(@"
     ╭───────────────────────────────────────────────╮
     │       CYBERSECURITY AWARENESS CHATBOT         │
     ╰───────────────────────────────────────────────╯
            ");
            Console.ResetColor();
        }

        private static void GetUserName()
        {
            DisplayColoredMessage("\nI'd like to get to know you better.", ConsoleColor.Yellow);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("What's your name? ");
            userName = Console.ReadLine()?.Trim() ?? "";
            Console.ResetColor();

            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = "Friend";
                DisplayColoredMessage("I'll call you Friend since you didn't provide a name.", ConsoleColor.Yellow);
            }
        }

        private static string GetResponse(string userInput)
        {
            foreach (var entry in responses)
            {
                if (userInput.ToLower().Contains(entry.Key))
                {
                    return $"{entry.Value}";
                }
            }

            return $"I'm not sure about that, {userName}. Could you try asking about password safety, phishing, or safe browsing?";
        }

        private static void DisplayColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void TypeWriterEffect(string message, int delayMilliseconds, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delayMilliseconds);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        private static void DisplayFormattedResponse(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            // Create a border with appropriate width
            string border = new string('─', message.Length + 6);
            Console.WriteLine($"╭{border}╮");
            Console.WriteLine($"│   {message}   │");
            Console.WriteLine($"╰{border}╯");

            Console.ResetColor();
        }
    }
}