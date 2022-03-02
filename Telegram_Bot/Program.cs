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
          
                    var message = update.Message;

                    ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new []
                    {
                        new KeyboardButton[] { "USD/PLN", "PLN/USD" },
                    })
                    {
                        ResizeKeyboard = true
                    };

                    await bot.SendTextMessageAsync(message.Chat.Id, "Check live foreign currency exchange rates. Select exchange currency:", replyMarkup: replyKeyboardMarkup);

                    switch (message.Text)
                    {
                        case "USD/PLN":
                            await bot.SendTextMessageAsync(message.Chat.Id, "USD/PLN - US Dollar Polish Zloty: 4.2914");
                            break;
                        case "PLN/USD":
                            await bot.SendTextMessageAsync(message.Chat.Id, "PLN/USD - Polish Zloty US Dollar: 4.2887");
                            break;
                        default:
                            await bot.SendTextMessageAsync(message.Chat.Id, "Select exchange currency:");
                            break;
                    }



                
        } 

       
    }
}

