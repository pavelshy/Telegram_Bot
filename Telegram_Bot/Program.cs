using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Telegram_Bot    
{
    class Program
    {
        static TelegramBotClient bot = new TelegramBotClient("5136381645:AAHMR6kHnYnhYUEPDV93qok88W_KPnb3AVA");
        
        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri("http://api.exchangeratesapi.io/v1/latest?access_key=0f10eb34fd242a0c0e0025707136b73d&symbols=USD,PLN&format=1");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var jsonDeserialize = System.Text.Json.JsonSerializer.Deserialize<Exchange>(json);
            }

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                }
            };

            bot.StartReceiving(updateHandel, errorHandler, receiverOptions);

            Console.ReadKey();
        }

        private static Task errorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task updateHandel(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
          if(update.Type == UpdateType.Message)
            {
                if(update.Message.Type == MessageType.Text)
                { 
                    var message = update.Message;

                    ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new []
                    {
                        new KeyboardButton[] { "Convert EUR/USD", "Convert EUR/PLN" },
                    })
                    {
                        ResizeKeyboard = true
                    };

                    await bot.SendTextMessageAsync(message.Chat.Id, "Check live foreign currency exchange rates. Select exchange currency:", replyMarkup: replyKeyboardMarkup);

                    switch (message.Text)
                    {
                        case "Convert EUR/USD":
                            await bot.SendTextMessageAsync(message.Chat.Id, $"1 EUR to USD - Convert Euros to US Dollars : 4.1");
                            break;
                        case "Convert EUR/PLN":
                            await bot.SendTextMessageAsync(message.Chat.Id, "1 EUR to PLN - Convert Euros to Polish Zlotych: 4.7");
                            break;
                    }
                    
                }
            }
        }
    }
    public class Exchange
    {
        public string date { get; set; }
        public Rates rates { get; set; }
    }
    public class Rates
    {
        [JsonPropertyName("USD")]
        public double EURUSD { get; set; }
        [JsonPropertyName("PLN")]
        public double EURPLN { get; set; }
    }
}

