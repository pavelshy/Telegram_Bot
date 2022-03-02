using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_Bot    // How I can send message ?
{
    struct BotUpdate
    {
        public string text;
        public long id;
        public string? username;
        public string? firstname;
        public DateTime? forwarddate; // forwarddate : null
    }
    
    class Program
    {
        static TelegramBotClient bot = new TelegramBotClient("5136381645:AAHMR6kHnYnhYUEPDV93qok88W_KPnb3AVA");
        static string fileName = "/Users/pavelshymko/Documents/NetFramework/Telegram_Bot/updates.json";
        static List<BotUpdate> botUpdates = new List<BotUpdate>();
        

        static void Main(string[] args)
        {
            
            // read all saved files
            try
            {
                var botUpdatesString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdatesString) ??
                    botUpdates;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error reading or desearializing {ex}");
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
                    //write an update
                    var _botUpdate = new BotUpdate
                    {
                        text = update.Message.Text,
                        id = update.Message.Chat.Id,
                        username = update.Message.Chat.Username,
                        firstname = update.Message.Chat.FirstName,
                        forwarddate = update.Message.ForwardDate
                    };

                    botUpdates.Add(_botUpdate);

                    var botUpdatesString = JsonConvert.SerializeObject(botUpdates);

                    System.IO.File.WriteAllText(fileName, botUpdatesString);
                }
            }  
        }
    }
}

