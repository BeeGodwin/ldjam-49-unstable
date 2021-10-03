using System;
using System.Collections.Generic;
using Bee.Ocean;
using UnityEngine;

namespace Bee.Weather
{
    public class WeatherController : MonoBehaviour
    {
        public float dayLength;
        public Forecast forecast;

        private IWeather _ocean;

        private WeatherGenerator _generator;
        
        private float _dayTimer = 0;
        private int _dayCounter = 0;
        private float _hourLength;
        private int _hourIndex = 0;
        private List<Hour> _hours;


        public void Start()
        {
            _generator = new WeatherGenerator();
            _hourLength = dayLength / 12;
            _ocean = GameObject.Find("Ocean").GetComponent<OceanController>();
            StartDay();
        }

        private void StartDay()
        {
            // TODO: control the sky
            Debug.Log($"Starting day with forecast {forecast}");
            GenerateWeather();
            SetWeather(_hours[0].Conditions);
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
            _dayTimer += Time.deltaTime;
            var thisHourIndex = (int)Mathf.Floor(_dayTimer / _hourLength);
            if (thisHourIndex != _hourIndex)
            {
                _hourIndex = thisHourIndex;
                SetWeather(_hours[_hourIndex].Conditions);
            }
            // TODO: end the day, generate a new one
        }

        private void SetWeather(WeatherConditions conditions)
        {
            _ocean.SetWeatherConditions(conditions);
        }
    }
}

public interface IWeather
{
    public void SetWeatherConditions(WeatherConditions conditions);
}

public enum Forecast
{
    Dry,
    Fair,
    Change,
    Rain,
    Storms
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

struct Hour
{
    public Hour(WeatherConditions weather)
    {
        Conditions = weather;
    }

    public WeatherConditions Conditions { get; }
}