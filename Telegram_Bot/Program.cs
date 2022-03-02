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
                    await bot.SendTextMessageAsync(message.Chat.Id, "Check live foreign currency exchange rates");
                    ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new []
                    {
                        new KeyboardButton[] { "USD/PLN", "PLN/USD" },
                    }
                        )
                    {
                        ResizeKeyboard = true
                    };

                    await bot.SendTextMessageAsync(message.Chat.Id, "Select exchange currency", replyMarkup: replyKeyboardMarkup);
                }
            }
        }

       
    }
}

