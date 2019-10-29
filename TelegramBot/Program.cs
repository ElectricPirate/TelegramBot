using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. What do you want to ask?");

            var QuestionsAnswersFile = File.ReadAllText(@"C:\Users\fromt\source\repos\TelegramBot\TelegramBot\QuestionsAnswers.json");
            var QuestionsAnswers = JsonConvert.DeserializeObject<Dictionary<string, string>>(QuestionsAnswersFile);

            while (true)
            {
                var UserQuestion = Console.ReadLine();
                var Answers = new List<string>();

                foreach (var entry in QuestionsAnswers)
                {
                    if (UserQuestion.ToLower().Contains(entry.Key.ToLower()))
                    {
                        Answers.Add(entry.Value);
                    }
                }

                if (UserQuestion.ToLower().Contains("what time is it"))
                {
                    Answers.Add($"Current time: {DateTime.Now.ToString("HH:mm")}");                    
                }

                if (UserQuestion.ToLower().Contains("what day is it"))
                {
                    Answers.Add($"Today: {DateTime.Now.ToString("dd.MM.yyyy")}");
                }

                if (UserQuestion.ToLower().Contains("bye"))
                {
                    Console.WriteLine("I'm tired anyway, so bye.");
                    break;
                }

                var result = String.Join(", ", Answers);
                Console.WriteLine($"Bot says: {result}");
            }

            Console.ReadKey();            
        }
    }
}
