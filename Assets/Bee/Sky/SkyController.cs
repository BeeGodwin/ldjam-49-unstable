using UnityEngine;

namespace Bee.Sky
{
    public class SkyController : MonoBehaviour, IWeather
    {
        private CloudController _clouds;
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
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            throw new System.NotImplementedException();
        }
    }
}
