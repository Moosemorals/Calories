using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Lib
{
    public static class TempDataExtensions
    {

        public static void AddMessage(this ITempDataDictionary tempData, string format, params object[] args)
        {
            List<Message> messages = GetMessages(tempData);
            messages.Add(new Message { Text = string.Format(format, args) });


            tempData.Add(Static.KeyMessages, JsonConvert.SerializeObject(messages));
        }

        public static List<Message> GetMessages(this ITempDataDictionary tempData)
        {
            List<Message> messages = new List<Message>();

            if (tempData.ContainsKey(Static.KeyMessages))
            {
                messages.AddRange(JsonConvert.DeserializeObject<List<Message>>((string)tempData[Static.KeyMessages]));
            }

            return messages;
        }

    }
}
