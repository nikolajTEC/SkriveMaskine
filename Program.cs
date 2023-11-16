using System;
using System.Diagnostics;
using System.Threading.Tasks.Sources;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace SkriveMaskine
{
    internal class Program
    {
        static char[] lowercaseLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'æ', 'ø', 'å' };
        static int correctCounter = 0;
        static int incorrectCounter = 0;
        static int[] highscore = new int[10];
        static List<string> lettersUsed = new List<string>();
        static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            #region Outcommented code if user should decide amount of characters.
            //Console.WriteLine("Hvor mange karakterer vil du skrive?");
            //string input = Console.ReadLine();
            //// Convert the string to an integer
            //if (int.TryParse(input, out int amountOfLetters))
            //{
            //    // Use the 'amountOfLetters' variable as an integer
            //    Console.WriteLine("You entered: " + amountOfLetters);
            //}
            //else
            //{
            //    Console.WriteLine("Invalid input. Please enter a valid integer.");
            //}
            #endregion
            int amountOfLetters = 10;
            highscore = LoadHighscore();
            do
            {
                ProgramStart(amountOfLetters);
                Show("vil du prøve igen J/N", 0, 0, ConsoleColor.White);
            }
            while (Console.ReadKey().Key == ConsoleKey.J);
        }

        private static int[] LoadHighscore()
        {
                    var serializer = new XmlSerializer(typeof(int[]));
            if (!File.Exists("scores.txt"))
            {
                using (var stream = File.Create("highscore.xml"))
                {
                    serializer.Serialize(stream, highscore);
                }
            }
            else
            {
            }
            throw new NotImplementedException();
        }
        static void saveHighscore(int[] highscore)
        {

        }

        private static void ProgramStart(int amountOfLetters)
        {
            Console.Clear();
            Show("Velkommen til skrivemaskine kursus, tryk j for at starte", 35, 5, ConsoleColor.Red);
            if (Console.ReadKey().Key == ConsoleKey.J)
            {
                stopwatch.Start();
                GenerateRandomLetters(amountOfLetters);
                Console.Clear();
                stopwatch.Stop();
                ProgramSlut();
            }

        }

        private static void GenerateRandomLetters(int amountOfLetters)
        {
            int score = 0;
            for (int i = 0; i < amountOfLetters; i++)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, lowercaseLetters.Length);
                char randomLowercaseLetter = lowercaseLetters[randomIndex];
                lettersUsed.Add(randomLowercaseLetter.ToString());
                score += UserInput(randomLowercaseLetter);
            }

            AddToHighScore(score);
        }
        private static void ProgramSlut()
        {
            string test = string.Join(" ", lettersUsed);
            Show($"dine tastede bogstaver var {test}", 0, 0, ConsoleColor.White);
            ShowHighscore();
            Show($"du fik {correctCounter} rigtige, og {incorrectCounter} forkerte, det tog {stopwatch.Elapsed.TotalSeconds:F2} sekunder", 0, 20, ConsoleColor.Blue);
        }
        static void ShowHighscore()
        {
            Show("***HIGHSCORE***", 50, 2, ConsoleColor.Cyan);
            for (int i = highscore.Length - 1; i >= 0; i--)
            {
                if (highscore[i] != null)
                    Show(highscore[i], 50, 4 + i, ConsoleColor.Cyan);
            }
        }

        private static int UserInput(char randomLowercaseLetter)
        {
            int score = 0;
            Stopwatch keyStopwatch = new Stopwatch();
            keyStopwatch.Start();
            Show($"skriv {randomLowercaseLetter}", 50, 15, ConsoleColor.Green);
            char input = Console.ReadKey(true).KeyChar;

            bool isSame = Compare(input, randomLowercaseLetter);
            keyStopwatch.Stop();
            if (isSame)
            {
                score += Math.Max(0, 3000 - (int)keyStopwatch.ElapsedMilliseconds);
                correctCounter++;

            }
            Show(score, 0, 15, ConsoleColor.White);
            return score;
        }

        private static bool Compare(char input, char randomLowercaseLetter)
        {
            if (input == randomLowercaseLetter) return true;
            else return false;
        }
        static void AddToHighScore(int score)
        {
            bool isHighscore = false;
            foreach (int highscore in highscore)
            {
                if (highscore < score)
                {
                    isHighscore = true;
                    break;
                }
            }
            if (!isHighscore) return;

            highscore[9] = score;
            highscore = highscore.OrderByDescending(score => score).ToArray();
            saveHighscore(highscore);
            //TODO save new highscore
        }
        static void Show(object text, int locationX, int locationY, ConsoleColor color = ConsoleColor.White)
        {

            Console.ForegroundColor = color;
            Console.SetCursorPosition(locationX, locationY);
            Console.Write(text);
        }
    }
}