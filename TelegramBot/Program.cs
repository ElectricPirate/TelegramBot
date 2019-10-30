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
        const string QUETIONS_ANSWERS_DB = @"C:\Users\fromt\source\repos\TelegramBot\TelegramBot\QuestionsAnswers.json";
        private static Dictionary<string, string> QuestionsAnswers;
        static void Main(string[] args)
        {
            var QuestionsAnswersFile = File.ReadAllText(QUETIONS_ANSWERS_DB);
            QuestionsAnswers = JsonConvert.DeserializeObject<Dictionary<string, string>>(QuestionsAnswersFile);
            
            TelegramAPI Api = new TelegramAPI();
            while (true)
            {
                var updates = Api.GetUpdates();
                foreach(var update in updates)
                {
                    var answer = answerQuestion(update.message.text);
                    var message = $"Dear, {update.message.chat.first_name}, {answer}";
                    Api.SenMessage(message, update.message.chat.id);
                }               
            }            
        }

        private static string answerQuestion(string question)
        {
            if(question==null)
            {
                return "I dont't understand you";
            }

            var Answers = new List<string>();

            foreach (var entry in QuestionsAnswers)
            {
                if (question.ToLower().Contains(entry.Key.ToLower()))
                {
                    Answers.Add(entry.Value);
                }
            }

            if (question.ToLower().Contains("what time is it"))
            {
                Answers.Add($"Current time: {DateTime.Now.ToString("HH:mm")}");
            }

            if (question.ToLower().Contains("what day is it"))
            {
                Answers.Add($"Today: {DateTime.Now.ToString("dd.MM.yyyy")}");
            }

            if (question.ToLower().Contains("bye"))
            {
                Answers.Add("I'm tired anyway, so bye.");                
            }

            if (Answers.Count == 0)
            {
                Answers.Add("I dont't understand you");
            }

            var result = String.Join(", ", Answers);
            return result;
        }
    }
}
