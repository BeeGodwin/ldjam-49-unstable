using System.Collections.Generic;
using Bee.Ocean;
using Bee.Sky;
using UnityEngine;

namespace Bee.Weather
{
    public class WeatherController : MonoBehaviour
    {
        public float dayLength;
        public Forecast forecast;
        public float accuracy;
        
        private IWeather _ocean;
        private IWeather _sky;

        private WeatherGenerator _generator;
        
        private float _dayTimer = 0;
        private int _dayCounter = 0;
        private float _hourLength;
        private int _hourIndex = -1;
        private List<Hour> _hours;


        public void Start()
        {
            _generator = new WeatherGenerator();
            _hourLength = dayLength / 12;
            _ocean = GameObject.Find("Ocean").GetComponent<OceanController>();
            _sky = GameObject.Find("Sky").GetComponent<SkyController>();
        }

        private void StartDay()
        {
            forecast = CheckForecast();
            GenerateWeather();
            Debug.Log($"Starting day {_dayCounter} with forecast {forecast}");
        }

        private Forecast CheckForecast()
        {
            var roll = Random.Range(0f, 1f);
            if (roll < accuracy) return forecast;

            if (forecast == Forecast.Dry) return Forecast.Fair;
            if (forecast == Forecast.Storms) return Forecast.Rain;
            var changeBy = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
            return (Forecast) ((int) forecast + changeBy);
        }

        private void GenerateWeather()
        {
            _hours = new List<Hour>();
            for (int i = 0; i < 12; i++)
            {
                _hours.Add(new Hour(_generator.GetConditions(forecast)));
            }
        }

        public void Update()
        {
            if (_dayTimer == 0f) StartDay();
            
            _dayTimer += Time.deltaTime;
            var thisHourIndex = (int)Mathf.Floor(_dayTimer / _hourLength);
            
            if (thisHourIndex != _hourIndex)
            {
                _hourIndex++;
                if (_hourIndex >= _hours.Count)
                {
                    _dayCounter += 1;
                    _dayTimer = 0f;
                    _hourIndex = 0;
                    StartDay();
                }
                SetWeather(_hours[_hourIndex].Conditions);
            }
        }

        private void SetWeather(WeatherConditions conditions)
        {
            _ocean.SetWeatherConditions(conditions, _hourLength);
            _sky.SetWeatherConditions(conditions, _hourLength);
        }
    }
}

public interface IWeather
{
    public void SetWeatherConditions(WeatherConditions conditions, float time);
}

public enum Forecast
{
    Dry = 0,
    Fair = 1,
    Change = 2,
    Rain = 3,
    Storms = 4
}

public struct WeatherConditions
{
    public Rain Rain;
    public Wind Wind;

    public WeatherConditions(Rain rain, Wind wind)
    {
        Rain = rain;
        Wind = wind;
    }
}

public enum Rain
{
    None,
    LightRain,
    HardRain,
    TorrentialRain
}

public enum Wind
{
    None,
    Breezy,
    Windy,
    Gale
}

readonly struct Hour
{
    public Hour(WeatherConditions weather)
    {
        Conditions = weather;
    }

    public WeatherConditions Conditions { get; }
}