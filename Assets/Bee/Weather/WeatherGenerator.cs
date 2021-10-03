using UnityEngine;

namespace Bee.Weather
{
    public struct WeatherGenerator
    {
        
        public WeatherConditions GetConditions(Forecast forecast)
        {
            var rain = GetRain(forecast);
            var wind = GetWind(forecast);

            return new WeatherConditions(rain, wind) ;
        }

        private Rain GetRain(Forecast forecast)
        {
            var rainChance = ChanceOfRain(forecast);
            var roll = Random.Range(0f, 1f);

            if (roll > rainChance)
                return Rain.None;
            
            if (roll <= rainChance / 3)
                return Rain.TorrentialRain;
            
            if (roll <= rainChance / 2)
                return Rain.HardRain;
            
            return Rain.LightRain;
        }
        
        private Wind GetWind(Forecast forecast)
        {
            var windChance = ChanceOfWind(forecast);
            var roll = Random.Range(0f, 1f);

            if (roll > windChance)
                return Wind.None;
            
            if (roll <= windChance / 3)
                return Wind.Gale;
            
            if (roll <= windChance / 2)
                return Wind.Windy;
            
            return Wind.Breezy;
        }

        private float ChanceOfRain(Forecast forecast)
        {
            switch (forecast)
            {
                case Forecast.Dry:
                    return 0.05f;
                case Forecast.Fair:
                    return 0.10f;
                case Forecast.Change:
                    return 0.5f;
                case Forecast.Rain:
                    return 0.75f;
                case Forecast.Storms:
                    return 1.0f;
            }
            return 0f;
        }
        
        private float ChanceOfWind(Forecast forecast)
        {
            switch (forecast)
            {
                case Forecast.Dry:
                    return 0.15f;
                case Forecast.Fair:
                    return 0.25f;
                case Forecast.Change:
                    return 0.75f;
                case Forecast.Rain:
                    return 0.5f;
                case Forecast.Storms:
                    return 1.0f;
            }
            return 0f;
        }
    }
}