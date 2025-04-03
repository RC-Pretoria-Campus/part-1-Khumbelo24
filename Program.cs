using System;
using System.Collections.Generic;
using System.Threading;
using System.Speech.Synthesis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PROG_ASSIGN1
{
    /// <summary>
    /// Main program class for the Cybersecurity Awareness Chatbot.
    /// This chatbot provides information about various cybersecurity topics
    /// with an interactive and visually appealing console interface.
    /// </summary>
    internal class Program
    {
        // User's name storage where i put my user's name
        static string userName = "";

        // Dictionary to store chatbot responses with case-insensitive key comparison
        static Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // General chatbot information
            { "how are you", "I'm functioning perfectly! Thanks for asking." },
            { "what's your purpose", "I'm here to help you understand cybersecurity concepts and stay safe online." },
            { "what can I ask you about", "You can ask me about password safety, phishing awareness, safe browsing, and general cybersecurity tips!" },
            { "who made you", "I was created by Spiro as a cybersecurity awareness educational tool." },
            { "hello", "Hello there! How can I help with your cybersecurity questions today?" },
            { "hi", "Hi there! Ready to learn about staying safe online?" },
            
            // Password safety
            { "password", "Strong passwords are at least 12 characters long with a mix of uppercase, lowercase, numbers, and symbols. Avoid using personal information or common words." },
            { "how often should i change passwords", "It's recommended to change passwords every 3-6 months, and immediately if you suspect a breach." },
            { "password manager", "Password managers securely store all your passwords so you only need to remember one master password. They can also generate strong unique passwords for each site." },
            { "two factor", "Two-factor authentication adds an extra layer of security by requiring something you know (password) and something you have (like your phone) to log in." },
            { "2fa", "2FA (Two-Factor Authentication) significantly improves your account security by requiring a second verification method beyond just your password." },
            { "passphrase", "Passphrases are longer password alternatives made of multiple words. They're easier to remember but harder to crack, like 'correct-horse-battery-staple'." },
            
            // Phishing awareness
            { "phishing", "Phishing is when attackers pose as trusted entities to trick you into revealing sensitive information like passwords or credit card details." },
            { "identify phishing", "Look for suspicious sender emails, spelling/grammar errors, urgent requests, unusual links, and requests for personal information." },
            { "suspect phishing", "Don't click any links or download attachments. Report the email to your IT department or forward to your email provider's phishing report address." },
            { "spear phishing", "Spear phishing targets specific individuals with personalized messages using information gathered about you, making them harder to identify than general phishing attempts." },
            { "whaling", "Whaling is a type of phishing specifically targeting high-profile executives or other high-value targets with sophisticated social engineering tactics." },
            { "social engineering", "Social engineering manipulates people into breaking security procedures or revealing confidential information by exploiting human psychology rather than technical hacking." },
            
            // Safe browsing
            { "safe browsing", "Safe browsing is a matter of always using updated browsers, looking for HTTPS in the URL, being cautious with downloads, and considering using a VPN when on public Wi-Fi." },
            { "https", "HTTPS (Hypertext Transfer Protocol Secure) encrypts data between your browser and the website, protecting your information from eavesdroppers." },
            { "vpn", "A VPN (Virtual Private Network) encrypts your internet connection, protecting your privacy and allowing you to browse more securely, especially on public networks." },
            { "public wifi", "When using public WiFi, avoid accessing sensitive accounts, use a VPN, verify network names, disable sharing, and use HTTPS websites whenever possible." },
            { "cookies", "Cookies are small files websites store on your device to remember information about you. While useful, they can pose privacy concerns and should be managed carefully." },
            { "malware", "Malware is malicious software designed to damage or gain unauthorized access to your system. Protect yourself with antivirus software, careful downloading, and regular updates." },
            
            // Data protection
            { "backup", "Regularly back up important data to external drives or cloud services following the 3-2-1 rule: 3 copies, on 2 different media types, with 1 copy offsite." },
            { "encryption", "Encryption converts your data into a code to prevent unauthorized access. Use it for sensitive files, communications, and when transmitting data online." },
            { "privacy settings", "Regularly review and update privacy settings on your devices, browsers, and social media accounts to control what information you share." },
            
            // Mobile security
            { "mobile security", "Protect your mobile devices by using screen locks, keeping software updated, only installing apps from official stores, and being cautious with app permissions." },
            { "app permissions", "Only grant apps the permissions they need to function. Be suspicious if a simple app requests access to contacts, location, or other sensitive information." },

            // Additional responses for comprehensiveness
            { "ransomware", "Ransomware is malicious software that encrypts your files and demands payment for the decryption key. Protect yourself with regular backups, security updates, and caution when opening attachments." },
            { "firewall", "A firewall acts as a barrier between your device and the internet, monitoring and filtering traffic based on security rules to protect against unauthorized access." },
            { "antivirus", "Antivirus software detects, prevents, and removes malicious software. Keep it updated and run regular scans to protect your system from threats." },
            { "updates", "Software updates often contain security patches for vulnerabilities. Setting your devices to update automatically helps protect against known threats." }
        };

        // history of them chats to store previous interactions bettween the user and my bot
        static List<string> chatHistory = new List<string>();

        // now here is the list of cybersecurity tips for the random tip feature
        static List<string> securityTips = new List<string>
        {
            "Never use the same password across multiple accounts.",
            "Enable two-factor authenticasstion wherever possible.",
            "Be careful about the information you share on social media.",
            "Regularly update your software and operating systems.",
            "Always hover over links before clicking to verify their destination.",
            "Back up your important data regularly.",
            "Use a password manager to generate and store strong passwords.",
            "Be suspicious of unexpected emails, even if they appear to be from known contacts.",
            "Avoid using public WiFi for sensitive transactions without a VPN.",
            "Regularly check your accounts for unauthorized activity.",
            "Create unique security questions with answers that aren't easily found online.",
            "Consider using encrypted messaging apps for sensitive communications.",
            "Use a privacy-focused browser extension to block trackers and ads.",
            "Regularly clear your browser cookies and cache to protect your privacy.",
            "Disable Bluetooth and WiFi when not in use to reduce attack surface."
        };

        // scheme of colors for consistent UI
        private static readonly ConsoleColor PrimaryColor = ConsoleColor.Cyan;
        private static readonly ConsoleColor SecondaryColor = ConsoleColor.Yellow;
        private static readonly ConsoleColor AlertColor = ConsoleColor.Red;
        private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
        private static readonly ConsoleColor BorderColor = ConsoleColor.Blue;

        static void Main(string[] args)
        {
            // Configure console window
            Console.Title = "Spiro's Cybersecurity Awareness Chatbot";

            try
            {
                // Initializing the chatbot interface
                InitializeChatbot();

                // Main interaction loop
                RunMainChatLoop();
            }
            catch (Exception ex)
            {
                // Global exception handler for unexpected errors
                DisplayColoredMessage($"An unexpected error occurred: {ex.Message}", AlertColor);
                DisplayColoredMessage("Please restart the application.", SecondaryColor);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

      
        private static void InitializeChatbot()
        {
            // Play welcome speech in the begining using Speech Synthesis
            PlayWelcomeSpeech();

            // Display there ASCII art banner
            ShowAsciiArt();

            // welcome message
            DisplayColoredMessage("\n╭─────────────────────────────────────────────────╮", BorderColor);
            DisplayColoredMessage("│   Welcome to Spiro's Cybersecurity Chatbot!     │", PrimaryColor);
            DisplayColoredMessage("╰─────────────────────────────────────────────────╯", BorderColor);

            // Get user's name for personalized interaction
            GetUserName();

            // for displaying instructions and greeting
            DisplayColoredMessage($"\nHello {userName}! I'm here to help you stay safe online.", SuccessColor);
            TypeWriterEffect("You can ask me questions about password safety, phishing, safe browsing, and more.", 20, SecondaryColor);
            TypeWriterEffect("Type 'help' for a list of commands or 'exit' when you're done.", 20, SecondaryColor);

            // Show a random security tip to start
            ShowRandomTip();
        }

        
        // Main chat loop that processes user input and generates their responses
       
        private static void RunMainChatLoop()
        {
            bool continueChat = true;
            while (continueChat)
            {
                Console.WriteLine();
                DisplayColoredMessage("╭─────────────────────────────────────────────────╮", BorderColor);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("│ Your question: ");
                string userInput = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();
                DisplayColoredMessage("╰─────────────────────────────────────────────────╯", BorderColor);

                // Process user input with validation
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    DisplayColoredMessage("Please type a question. I'm here to help!", SecondaryColor);
                    continue;
                }

                // Add to chat history
                chatHistory.Add($"You: {userInput}");

                // Process commands
                switch (userInput.ToLower())
                {
                    case "exit":
                    case "quit":
                    case "bye":
                        continueChat = false;
                        DisplayColoredMessage($"\nThank you for chatting with me, {userName}! Stay safe online!", PrimaryColor);
                        continue;

                    case "help":
                        ShowHelpMenu();
                        continue;

                    case "clear":
                        Console.Clear();
                        ShowAsciiArt();
                        continue;

                    case "topics":
                        ShowTopics();
                        continue;

                    case "tip":
                        ShowRandomTip();
                        continue;

                    case "history":
                        ShowChatHistory();
                        continue;

                    case "save":
                        SaveChatHistory();
                        continue;
                }

                // Get and display response
                string response = GetResponse(userInput);
                DisplayFormattedResponse(response);

                // Add to chat history
                chatHistory.Add($"Bot: {response}");
            }
        }

        
        // Initializes and plays a voice greeting using Speech Synthesis
        
        private static void PlayWelcomeSpeech()
        {
            try
            {
                DisplayColoredMessage("Initializing voice greeting...", ConsoleColor.Gray);

                // Create a new SpeechSynthesizer instance
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {
                    // Configure the voice settings
                    synthesizer.Rate = 0;     // Normal speech type
                    synthesizer.Volume = 100; // Full volume of the bass voice

                    // Set the welcome message
                    string welcomeMessage = "Hello! Welcome to Spiro's Cybersecurity Awareness Bot. I'm here to help you stay safe online.";

                    // Speak the welcome message
                    synthesizer.Speak(welcomeMessage);

                    DisplayColoredMessage("Voice greeting complete.", ConsoleColor.Gray);
                }
            }
            catch (Exception ex)
            {
                // For handling speech synthesis errors gracefully
                DisplayColoredMessage($"Speech synthesis error: {ex.Message}", AlertColor);
                DisplayColoredMessage("Continuing without voice greeting. Please ensure your audio is enabled.", SecondaryColor);
            }
        }

       
        // Displays the ASCII art banner for the chatbot
       
        private static void ShowAsciiArt()
        {
            Console.ForegroundColor = PrimaryColor;
            Console.WriteLine(@"
    ███████╗██████╗ ██╗██████╗  ██████╗ ███████╗
    ██╔════╝██╔══██╗██║██╔══██╗██╔═══██╗██╔════╝
    ███████╗██████╔╝██║██████╔╝██║   ██║███████╗
    ╚════██║██╔═══╝ ██║██╔══██╗██║   ██║╚════██║
    ███████║██║     ██║██║  ██║╚██████╔╝███████║
    ╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝
                                              
     ╔═══════════════════════════════════════════╗
     ║      CYBERSECURITY CHATBOT                ║
     ╚═══════════════════════════════════════════╝
            ");
            Console.ResetColor();
        }

       
        // Gets the user's name with validation
       
        private static void GetUserName()
        {
            DisplayColoredMessage("\nI'd like to get to know you better.", SecondaryColor);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("What's your name? ");
            userName = Console.ReadLine()?.Trim() ?? "";
            Console.ResetColor();

            // Validate username - check for empty input or non-alphabetic characters (irrelevent stuff)
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = "Friend";
                DisplayColoredMessage("I'll call you Friend since you didn't provide a name.", SecondaryColor);
            }
            else if (!Regex.IsMatch(userName, @"^[a-zA-Z\s\-']+$"))
            {
                string oldName = userName;
                userName = "Friend";
                DisplayColoredMessage($"'{oldName}' contains unusual characters. I'll call you Friend for now.", SecondaryColor);
            }
        }

        private static string GetResponse(string userInput)
        {
            // Convert to lowercase for matching
            string normalizedInput = userInput.ToLower();

            // 1. For looking for exact matches
            foreach (var entry in responses)
            {
                if (normalizedInput == entry.Key.ToLower())
                {
                    return entry.Value;
                }
            }

            // 2. Then for keyword matches
            List<string> matchedResponses = new List<string>();
            foreach (var entry in responses)
            {
                if (normalizedInput.Contains(entry.Key.ToLower()))
                {
                    matchedResponses.Add(entry.Value);
                    // Limit to 2 topics for readability
                    if (matchedResponses.Count >= 2) break;
                }
            }

            // 3. If multiple matches found, join 
            if (matchedResponses.Count > 1)
            {
                return string.Join("\n\nAlso, ", matchedResponses);
            }
            else if (matchedResponses.Count == 1)
            {
                return matchedResponses[0];
            }

            // 4. Try word-by-word matching for more flexibility
            string[] words = normalizedInput.Split(new[] { ' ', ',', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (word.Length < 4) continue; // Skip short words

                foreach (var entry in responses)
                {
                    if (entry.Key.ToLower().Contains(word))
                    {
                        return entry.Value;
                    }
                }
            }

            // 5. No match found - provide helpful fallback response
            return $"I'm not sure about that, {userName}. Could you try asking about password safety, phishing, safe browsing, or type 'topics' to see what I can help with?";
        }

        
        // Displays a message in the right color        
        private static void DisplayColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        
        // Creates a typewriter effect by displaying text character by character
        
        // <param name="message">The message to display</param>
        // <param name="delayMilliseconds">Delay between characters</param>
       // <param name="color">Color to display the text in</param>
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

        /// Displays the chatbot response in a formatted box with borders
     
        /// <param name="message">The response message to display</param>
        private static void DisplayFormattedResponse(string message)
        {
            string[] lines = message.Split('\n');
            int maxLength = lines.Max(line => line.Length);

            Console.ForegroundColor = SuccessColor;

            // Create a border with appropriate width
            string border = new string('─', maxLength + 6);
            Console.WriteLine($"╭{border}╮");

            foreach (string line in lines)
            {
                // Calculate padding to center the text
                int padding = maxLength - line.Length;
                int leftPad = padding / 2;
                int rightPad = padding - leftPad;
                string paddedLine = new string(' ', leftPad + 3) + line + new string(' ', rightPad + 3);
                Console.WriteLine($"│{paddedLine}│");
            }

            Console.WriteLine($"╰{border}╯");
            Console.ResetColor();
        }

        
        // Displays a help menu with available commands
       
        private static void ShowHelpMenu()
        {
            Console.ForegroundColor = PrimaryColor;
            Console.WriteLine("\n╭───────────────────────────────────────────────────╮");
            Console.WriteLine("│               AVAILABLE COMMANDS                   │");
            Console.WriteLine("├───────────────────────────────────────────────────┤");
            Console.WriteLine("│ help    - Show this help menu                     │");
            Console.WriteLine("│ topics  - Show available cybersecurity topics     │");
            Console.WriteLine("│ tip     - Get a random cybersecurity tip          │");
            Console.WriteLine("│ clear   - Clear the console screen                │");
            Console.WriteLine("│ history - View your chat history                  │");
            Console.WriteLine("│ save    - Save your chat history to a file        │");
            Console.WriteLine("│ exit    - Exit the chatbot                        │");
            Console.WriteLine("╰───────────────────────────────────────────────────╯");
            Console.ResetColor();
        }

        /// <summary>
        /// Shows available topics organized by category
        /// </summary>
        private static void ShowTopics()
        {
            // Group responses by category based on their keys
            var topics = new Dictionary<string, List<string>>
            {
                { "General Information", new List<string> { "how are you", "what's your purpose", "what can I ask you about", "who made you", "hello", "hi" } },
                { "Password Safety", new List<string> { "password", "how often should i change passwords", "password manager", "two factor", "2fa", "passphrase" } },
                { "Phishing Awareness", new List<string> { "phishing", "identify phishing", "suspect phishing", "spear phishing", "whaling", "social engineering" } },
                { "Safe Browsing", new List<string> { "safe browsing", "https", "vpn", "public wifi", "cookies", "malware" } },
                { "Data Protection", new List<string> { "backup", "encryption", "privacy settings", "ransomware" } },
                { "Mobile Security", new List<string> { "mobile security", "app permissions" } }
            };

            Console.ForegroundColor = PrimaryColor;
            Console.WriteLine("\n╭───────────────────────────────────────────────────╮");
            Console.WriteLine("│             CYBERSECURITY TOPICS                   │");
            Console.WriteLine("├───────────────────────────────────────────────────┤");

            foreach (var category in topics)
            {
                Console.WriteLine($"│ {category.Key}:");
                foreach (var topic in category.Value)
                {
                    Console.WriteLine($"│   - {topic}");
                }
                Console.WriteLine("│");
            }

            Console.WriteLine("│ Try asking about any of these topics!              │");
            Console.WriteLine("╰───────────────────────────────────────────────────╯");
            Console.ResetColor();
        }

        
        // Displays a random cybersecurity tip from the predefined list
       
        private static void ShowRandomTip()
        {
            Random random = new Random();
            int tipIndex = random.Next(securityTips.Count);

            Console.WriteLine();
            DisplayColoredMessage("╭───────────────────────────────────────────────────╮", SecondaryColor);
            DisplayColoredMessage("│                  SECURITY TIP                     │", SecondaryColor);
            DisplayColoredMessage("├───────────────────────────────────────────────────┤", SecondaryColor);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"│ {securityTips[tipIndex],-51} │");
            DisplayColoredMessage("╰───────────────────────────────────────────────────╯", SecondaryColor);
        }

        
        // Displays the chat history in a formatted view
        
        private static void ShowChatHistory()
        {
            if (chatHistory.Count == 0)
            {
                DisplayColoredMessage("No chat history available yet.", SecondaryColor);
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n╭───────────────────────────────────────────────────╮");
            Console.WriteLine("│                 CHAT HISTORY                       │");
            Console.WriteLine("├───────────────────────────────────────────────────┤");

            foreach (string entry in chatHistory)
            {
                Console.WriteLine($"│ {entry}");
                Console.WriteLine("│");
            }

            Console.WriteLine("╰───────────────────────────────────────────────────╯");
            Console.ResetColor();
        }

        
        // Saves the chat history to a text file with error handling
        
        private static void SaveChatHistory()
        {
            if (chatHistory.Count == 0)
            {
                DisplayColoredMessage("No chat history available to save.", SecondaryColor);
                return;
            }

            try
            {
                // This here now creates a unique filename based on current date and time
                string fileName = $"ChatHistory_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                // This writes chat history to file
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"Spiro's Cybersecurity Chatbot - Chat with {userName}");
                    writer.WriteLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("---------------------------------------------");

                    foreach (string entry in chatHistory)
                    {
                        writer.WriteLine(entry);
                        writer.WriteLine();
                    }
                }

                DisplayColoredMessage($"Chat history saved to {fileName}", SuccessColor);
            }
            catch (Exception ex)
            {
                // Handle file's I/O errors nicely
                DisplayColoredMessage($"Error saving chat history: {ex.Message}", AlertColor);
                DisplayColoredMessage("Please make sure you have write permissions in this directory.", SecondaryColor);
            }
        }
    }
}