using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    internal class OdessaWeather
    {
        public static string FilePath = "https://api.open-meteo.com/v1/forecast?latitude=46.4857&longitude=30.7438&current=temperature_2m&daily=temperature_2m_max,temperature_2m_min,precipitation_probability_max&forecast_days=1";
        public static string FileTimePath = "https://www.timeapi.io/api/time/current/coordinate?latitude=48.5833&longitude=39.3333";

    }
}
