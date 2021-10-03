using UnityEngine;

namespace Bee.Sky
{
    public class SkyController : MonoBehaviour, IWeather
    {
        private IWeather _clouds;
        private RainController _rain;
        private WindController _wind;
        private DayNightController _dayNight;
        
        void Start()
        {
            // get child controllers
            _clouds = GetComponentInChildren<CloudController>();
            _rain = GetComponentInChildren<RainController>();
            _wind = GetComponentInChildren<WindController>();
            _dayNight = GetComponentInChildren<DayNightController>();
            _dayNight.StartDay();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            // Debug.Log($"Sky got rain {conditions.Rain}, wind {conditions.Wind}");
            _dayNight.StartNextHour(time);
            _clouds.SetWeatherConditions(conditions, time);
            _rain.SetRainConditions(conditions, time);
            _wind.SetWindConditions(conditions.Wind, time);
        }
    }
}
