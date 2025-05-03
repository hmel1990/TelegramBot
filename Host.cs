using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public class Host
    {
        public Action <ITelegramBotClient, Update> ? OnMessage;

        private TelegramBotClient _bot;
        public Host(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        public void Start()
        {
            _bot.StartReceiving(UpdateHandler, ErrorHandler);
        }

        private async Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            Console.WriteLine("Ошибка: " + exception.Message);
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            OnMessage?.Invoke(client, update);
            Console.WriteLine(update.Message?.Text ?? "не текст");
        }
    }
}
