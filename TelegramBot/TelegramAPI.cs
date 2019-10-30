using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot
{
    public class TelegramAPI
    {
        public class ApiResult
        {
            public Update[] result { get; set; }
        }

        public class Update
        {
            public int update_id { get; set; }
            public Message message { get; set; }
        }

        public class Message
        {
            public Chat chat { get; set; }
            public string text { get; set; }
        }

        public class Chat
        {
            public int id { get; set; }
            public string first_name { get; set; }
        }

        private int lastUpdateId = 0;
        RestClient RC = new RestClient();
        const string API_URL = "https://api.telegram.org/bot" + Token.API_KEY + "/";

        public TelegramAPI() { }

        public string SendApiRequest(string apiMethod, string Params)
        {
            var Url = API_URL + apiMethod + "?" + Params;

            var Request = new RestRequest(Url);

            var Response = RC.Get(Request);

            return Response.Content;
        }

        public void SenMessage(string text,int chat_id)
        {
            SendApiRequest("sendMessage", $"chat_id={chat_id}&text={text}");
        }

        public Update[] GetUpdates()
        {
            var json = SendApiRequest("getUpdates", $"offset={lastUpdateId}");
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(json);

            foreach(var update in apiResult.result)
            {
                try
                {
                    Console.WriteLine($"Получен апдейт {update.update_id}, "
                        + $"сообщение от {update.message.chat.first_name}, "
                        + $"текст: {update.message.text}"
                        );                    
                }
                catch
                {
                    continue;
                }
                finally
                {
                    lastUpdateId = (int)(update.update_id + 1);
                }

            }
            return apiResult.result;
        }
        
    }
}
