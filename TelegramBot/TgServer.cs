using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using DataLayer;

namespace TelegramBot
{
    public class TgServer
    {
        TelegramBotClient TgBot;
        IServiceProvider _serviceProvider;
        public TgServer(IServiceProvider serviceProvider)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "HomeAccountingTgBot","token");
            var tgapi = System.IO.File.ReadAllText(path);
            TgBot = new TelegramBotClient(tgapi);
            _serviceProvider = serviceProvider;


        }
        public void TgStart()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            TgBot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }
        public void TgStop() => TgBot.CloseAsync();
        private IReplyMarkup GetStartButtons()
        {
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton ("О программе"), new KeyboardButton ("Команды") },
                    new List<KeyboardButton>{ new KeyboardButton ("Формат записей") },
                    new List<KeyboardButton>{ new KeyboardButton ("Сделать отчёт") }


                }
            );
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
    CancellationToken cancellationToken)
        {
            var b = _serviceProvider.GetService<AccountingDbContext>();
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
        }

    }
}
