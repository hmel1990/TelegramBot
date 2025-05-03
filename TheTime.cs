using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelegramBot
{
    internal class TheTime
    {
     public int Hour { get; set; }
     public int Minute { get; set; }




        //читаем по ссылке и сохраняем в строку
        private async Task<string> ReadJsonFileAsync(string FilePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(FilePath);
                    response.EnsureSuccessStatusCode();
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}\n{ex.StackTrace}");
                return "";
            }
        }

        // Парсим JSON в объект
        public async Task DeserializeToTime(string FilePath)
        {
            string json = await ReadJsonFileAsync(FilePath);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Ошибка: пустой JSON");
            }
            var data = JsonSerializer.Deserialize<AdditionalTime>(json);

            if (data == null)
            {
                Console.WriteLine("Ошибка десериализации JSON");
            }

            Hour = data.Hour;
            Minute = data.Minute;


        }
        //,
        public static async Task<TheTime> CreateAsyncTime(string FilePath)
        {
            var instance = new TheTime();
            await instance.DeserializeToTime(FilePath);
            return instance;
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}";
        }

        public class AdditionalTime
        {
            [JsonPropertyName("year")] public int Year { get; set; }
            [JsonPropertyName("month")] public int Month { get; set; }
            [JsonPropertyName("day")] public int Day { get; set; }
            [JsonPropertyName("hour")] public int Hour { get; set; }
            [JsonPropertyName("minute")] public int Minute { get; set; }
            [JsonPropertyName("seconds")] public int Seconds { get; set; }
            [JsonPropertyName("milliSeconds")] public int MilliSeconds { get; set; }
            [JsonPropertyName("dateTime")] public string DateTime { get; set; }
            [JsonPropertyName("date")] public string Date { get; set; }
            [JsonPropertyName("time")] public string Time_ { get; set; }
            [JsonPropertyName("timeZone")] public string TimeZone { get; set; }
            [JsonPropertyName("dayOfWeek")] public string DayOfWeek { get; set; }
            [JsonPropertyName("dstActive")] public bool DstActive { get; set; }
        }
    }

    
}
