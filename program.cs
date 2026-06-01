//pls dont cheat and change seconds on words! just add ur words if u want(json files)
//play fun!
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace TypingGame
{
    public class WordEntry
    {
        public string Word { get; set; }
        public int TimeSeconds { get; set; }
    }

    class Program
    {
        static string ReadLineWithTimeout(int timeoutSeconds)
        {
            StringBuilder sb = new StringBuilder();
            Stopwatch sw = Stopwatch.StartNew();
            bool enterPressed = false;

            while (sw.Elapsed.TotalSeconds < timeoutSeconds)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        enterPressed = true;
                        Console.WriteLine();
                        break;
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Length--;
                            Console.Write("\b \b");
                        }
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        sb.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar);
                    }
                }
                Thread.Sleep(10);
            }

            if (!enterPressed)
            {
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
                return null;
            }

            return sb.ToString();
        }

        static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static void Main(string[] args)
        {
            string level = "";
            while (true)
            {
                Console.Write("Выберите уровень сложности (easy, medium, hard): ");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "easy" || input == "medium" || input == "hard")
                {
                    level = input;
                    break;
                }
                Console.WriteLine("Неверный выбор. Введите 'easy', 'medium' или 'hard'.\n");
            }

            string jsonFilePath = $"{level}words.json";

            List<WordEntry> words;
            try
            {
                string jsonText = File.ReadAllText(jsonFilePath);
                words = JsonSerializer.Deserialize<List<WordEntry>>(jsonText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла {jsonFilePath}: {ex.Message}");
                return;
            }

            if (words == null || words.Count == 0)
            {
                Console.WriteLine($"Файл {jsonFilePath} пуст или не содержит слов.");
                return;
            }

            Shuffle(words);

            Console.WriteLine($"\nУровень: {level}. Игра началась! Чтобы выйти досрочно, введите 'quit'.\n");

            int score = 0;

            for (int i = 0; i < words.Count; i++)
            {
                var entry = words[i];

                Console.WriteLine($"Слово: {entry.Word}   (у вас {entry.TimeSeconds} сек.)");
                Console.Write("> ");

                string userInput = ReadLineWithTimeout(entry.TimeSeconds);

                if (userInput != null && userInput.Trim().Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Выход из игры.");
                    break;
                }

                if (userInput == null)
                {
                    Console.WriteLine("Не успел! Время вышло.\n");
                }
                else if (!string.Equals(userInput.Trim(), entry.Word, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Неверно написано. Счёт: {score}\n");
                }
                else
                {
                    score++;
                    Console.WriteLine($"Верно! +1 балл (счёт: {score})\n");
                }

                if (i < words.Count - 1)
                {
                    Console.WriteLine("Лёгкий 2,2-секундный отдых...");
                    Thread.Sleep(2200);
                }
            }

            Console.WriteLine($"Игра окончена. Итоговый счёт: {score}");
        }
    }
}

//React and Type EN version
//if u cant write a word - admit that you're a loser
//This game is not a typical browser game where you test your typing speed on a keyboard.
//this game is about ur reaction and fast typing
// If u are a programmer - u will finish hard mode with 90%+ score\
//If u are the programmer - u will finish this game with 100% score
//if u are noob u also can switch seconds on words(cheating)
//if words r very ez u can change sec's on words
//if the 2.2 second break is long or short, you can change the value on line 153
