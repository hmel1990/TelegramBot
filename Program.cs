using System;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;



namespace TelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");

if (string.IsNullOrWhiteSpace(token))
{
    Console.WriteLine("❌ TELEGRAM_TOKEN не задан");
    return;
}

            Host maxClient = new Host(token);
            maxClient.Start();
            maxClient.OnMessage += OnMessage;
            maxClient.OnMessage += HelloMessage;
            maxClient.OnMessage += WeatherInLuhansk;
            maxClient.OnMessage += WeatherInOdessa;
            maxClient.OnMessage += TimeInOdessa;



            //var OW = await CityWeather.CreateAsyncOdessaWeather(LuhanskWeather.FilePath);
            //Console.WriteLine(OW);

            await Task.Delay(-1);
        }





        private static async void TimeInLuhansk(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.Contains("Показать время в Луганске"))
            {
                var LuhanskTime = await TheTime.CreateAsyncTime(LuhanskWeather.FileTimePath);
                LuhanskTime.Hour = LuhanskTime.Hour + 1;
                //Console.WriteLine(OW);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Текущее время в Луганске: {LuhanskTime}");
            }
        }

        private static async void TimeInOdessa(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.Contains("Показать время в Одессе"))
            {
                var OdessaTime = await TheTime.CreateAsyncTime(OdessaWeather.FileTimePath);
                //Console.WriteLine(OW);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Текущее время в Одессе: {OdessaTime}");
            }
        }
        private static async void WeatherInLuhansk(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.Contains("Показать погоду в Луганске"))
            {
                var luhanskWeather = await CityWeather.CreateAsyncOdessaWeather(LuhanskWeather.FilePath);
                //Console.WriteLine(OW);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"{luhanskWeather}");

            }
        }

        private static async void WeatherInOdessa(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.Contains("Показать погоду в Одессе"))
            {
                var luhanskWeather = await CityWeather.CreateAsyncOdessaWeather(OdessaWeather.FilePath);
                //Console.WriteLine(OW);
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"{luhanskWeather}");

            }
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.ToLower().Contains(""))
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Выбери действие из меню ниже 👇", replyMarkup: GetButtons());
            }
        }

        private static IReplyMarkup? GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> // Первый ряд
            {
                new KeyboardButton { Text = "Показать погоду в Луганске" },
                new KeyboardButton { Text = "Показать погоду в Одессе" }
            },
            new List<KeyboardButton> // Второй ряд
            {
                new KeyboardButton { Text = "Показать время в Луганске" },
                new KeyboardButton { Text = "Показать время в Одессе" }
            }
        },
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };
        }

        private static async void HelloMessage(ITelegramBotClient client, Update update)
        {
            if (update.Message.Text.ToLower().Contains("привет"))
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Привет, как дела?!!!");
            }    
        }
    }

       
}
