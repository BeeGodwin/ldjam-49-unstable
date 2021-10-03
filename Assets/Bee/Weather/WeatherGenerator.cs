using UnityEngine;

namespace Bee.Weather
{
    public struct WeatherGenerator
    {
        
        public WeatherConditions GetConditions(Forecast forecast)
        {
            var rain = GetRain(forecast);
            var wind = GetWind(forecast);
            var swell = GetSwell(forecast);

            return rain | wind | swell;
        }

        private WeatherConditions GetRain(Forecast forecast)
        {
            var rainChance = ChanceOfRain(forecast);
            var roll = Random.Range(0f, 1f);

            if (roll > rainChance)
                return WeatherConditions.None;
            
            if (roll <= rainChance / 3)
                return WeatherConditions.TorrentialRain;
            
            if (roll <= rainChance / 2)
                return WeatherConditions.HardRain;
            
            return WeatherConditions.LightRain;
        }
        
        private WeatherConditions GetWind(Forecast forecast)
        {
            var windChance = ChanceOfWind(forecast);
            var roll = Random.Range(0f, 1f);

            if (roll > windChance)
                return WeatherConditions.None;
            
            if (roll <= windChance / 3)
                return WeatherConditions.Gale;
            
            if (roll <= windChance / 2)
                return WeatherConditions.Windy;
            
            return WeatherConditions.Breezy;
        }
        
        private WeatherConditions GetSwell(Forecast forecast)
        {
            var windChance = ChanceOfSwell(forecast);
            var roll = Random.Range(0f, 1f);

            if (roll > windChance)
                return WeatherConditions.None;
            
            if (roll <= windChance / 2)
                return WeatherConditions.BigSwell;

            return WeatherConditions.MildSwell;
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
        
        private float ChanceOfSwell(Forecast forecast)
        {
            switch (forecast)
            {
                case Forecast.Dry:
                    return 0.05f;
                case Forecast.Fair:
                    return 0.20f;
                case Forecast.Change:
                    return 0.25f;
                case Forecast.Rain:
                    return 0.25f;
                case Forecast.Storms:
                    return 0.5f;
            }
            return 0f;
        }
    }
}