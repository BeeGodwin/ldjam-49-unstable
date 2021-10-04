using Bee.Game;
using UnityEngine;

namespace Bee.Sky
{
    public class SkyController : MonoBehaviour, IWeather, IGameSystem
    {
        private IWeather _clouds;
        private RainController _rain;
        private WindController _wind;
        private DayNightController _dayNight;
        private BackdropController _backdrop;
        
        void Start()
        {
            _clouds = GetComponentInChildren<CloudController>();
            _rain = GetComponentInChildren<RainController>();
            _wind = GetComponentInChildren<WindController>();
            _dayNight = GetComponentInChildren<DayNightController>();
            _backdrop = GetComponentInChildren<BackdropController>();
        }

        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            // Debug.Log($"Sky got rain {conditions.Rain}, wind {conditions.Wind}");
            _dayNight.StartNextHour(time);
            _backdrop.SetWeatherConditions(conditions, time);
            _clouds.SetWeatherConditions(conditions, time);
            _rain.SetRainConditions(conditions, time);
            _wind.SetWindConditions(conditions.Wind, time);
        }

        public void PlayGame()
        {
            // TODO: what goes here
        }

        public void PauseGame()
        {
            // TODO: what goes here
        }

        public void ResetGame()
        {
            // TODO: what goes here
        }
    }
}
