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
            Debug.Log($"Starting day with forecast {forecast} and ocean {_ocean}");
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

[Flags]
public enum WeatherConditions
{
    None = 0b_0000_0000,
    LightRain = 0b_0000_0001,
    HardRain = 0b_0000_0010,
    TorrentialRain = 0b_0000_0100,
    Breezy = 0b_0000_1000,
    Windy = 0b_0001_0000,
    Gale = 0b_0010_0000,
    MildSwell = 0b_0100_0000,
    BigSwell = 0b_1000_0000
}

struct Hour
{
    public Hour(WeatherConditions weather)
    {
        Conditions = weather;
    }

    public WeatherConditions Conditions { get; }
}