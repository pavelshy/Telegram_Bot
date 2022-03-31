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
using System.IO;
using System.Text;


namespace Telegram_Bot    
{
    class Program
    {
        static TelegramBotClient bot = new TelegramBotClient("5136381645:AAHMR6kHnYnhYUEPDV93qok88W_KPnb3AVA");
        

        static void Main(string[] args)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                }
            };

            bot.StartReceiving(UpdateHandel, ErrorHandler, receiverOptions);

            Console.ReadKey();
        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandel(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            var message = update.Message;
            string path = $"/Users/pavelshymko/Documents/NetFramework/Telegram_Bot/{DateTime.Today.ToString("ddMMyyyy")}.json";

            var client = new HttpClient();
            var endpoint = new Uri("http://api.exchangeratesapi.io/v1/latest?access_key=0f10eb34fd242a0c0e0025707136b73d&symbols=USD,PLN&format=1");
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            var jsonDeserialize = System.Text.Json.JsonSerializer.Deserialize<Exchange>(json);
            
            try
            {
                System.IO.File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing: {ex}");
            }

            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                   new KeyboardButton[] { "Convert EUR/USD", "Convert EUR/PLN"},
            })
            {
                ResizeKeyboard = true

            };

            await bot.SendTextMessageAsync(message.Chat.Id, "Check live foreign currency exchange rates:", replyMarkup: replyKeyboardMarkup);

            switch (message.Text)
            {
                case "Convert EUR/USD":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"1 EUR to USD: {jsonDeserialize.rates.EURUSD} \nDate: {jsonDeserialize.date}");
                    var enterAmount = await bot.SendTextMessageAsync(message.Chat.Id, "Enter amount EUR: ");
                    

                    // double sumConvertEURUSD = jsonDeserialize.rates.EURUSD * jsonDeserialize.rates.EURUSD;
                    // await bot.SendTextMessageAsync(message.Chat.Id, $"Convert:{sumConvertEURUSD}");
                    
                    break;
                case "Convert EUR/PLN":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"1 EUR to PLN: {jsonDeserialize.rates.EURPLN} \nDate: {jsonDeserialize.date}");
                    break;
                default:
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Select exchange currency:");
                    break;
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

