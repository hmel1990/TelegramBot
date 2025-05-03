using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// Из-за того что принимаемый json имеет сложную структуру
// мы сначала создаем классы а внутри классов еще вспомогательные классы и в них поля для каждого элемента json
// затем получаем строку из метода ReadJsonFileAsync
// строку передаем в метод DeserializeToWeather который парсит джейсон в объект WeatherResponse из которого копируем поля в наш объект CityWeather
// переопределяем метод ToString
// метод CreateAsyncOdessaWeather создает внутри себя объект CityWeather
// у которого вызывается метод DeserializeToWeather который и заполняет поля этого созданного объекта



namespace TelegramBot
{
    internal class CityWeather
    {     
        [JsonIgnore] // Не участвует в сериализации/десериализации
        public double TemperatureCurrent { get; set; }

        [JsonIgnore]
        public double TemperatureMax { get; set; }

        [JsonIgnore]
        public double TemperatureMin { get; set; }

        [JsonIgnore]
        public double PrecipitationProbability { get; set; }

   
        //__________________________________________________________________________

        public static async Task <CityWeather> CreateAsyncOdessaWeather(string FilePath)
        {
            var instance = new CityWeather();
            await instance.DeserializeToWeather(FilePath);
            return instance;
        }

        //__________________________________________________________________________


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
        public async Task DeserializeToWeather(string FilePath)
        {
            string json = await ReadJsonFileAsync(FilePath);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Ошибка: пустой JSON");
                return;
            }

            var data = JsonSerializer.Deserialize<WeatherResponse>(json);

            if (data == null)
            {
                Console.WriteLine("Ошибка десериализации JSON");
            }



            TemperatureCurrent = data.Current.Temperature_2m;
            TemperatureMax = data.Daily.Temperature_2m_Max[0];
            TemperatureMin = data.Daily.Temperature_2m_Min[0];
            PrecipitationProbability = data.Daily.Precipitation_Probability_Max[0];

        }

        public override string ToString()
        {
            return 
                $"Текущая температура: {TemperatureCurrent}\n" +
                $"Максимальная температура: {TemperatureMax}\n" +
                $"Минимальная температура: {TemperatureMin}\n" +
                $"Вероятность осадков: {PrecipitationProbability}";
        }

    }

    //=============================================================================
    // Вспомогательные классы для десериализации JSON
    public class WeatherResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather Current { get; set; }

        [JsonPropertyName("daily")]
        public DailyWeather Daily { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature_2m { get; set; }
    }

    public class DailyWeather
    {
        [JsonPropertyName("temperature_2m_max")]
        public List<double> Temperature_2m_Max { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double> Temperature_2m_Min { get; set; }

        [JsonPropertyName("precipitation_probability_max")]
        public List<double> Precipitation_Probability_Max { get; set; }
    }

    //=============================================================================
}
