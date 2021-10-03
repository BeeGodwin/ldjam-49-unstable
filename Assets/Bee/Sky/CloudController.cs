using UnityEngine;

namespace Bee.Sky
{
    public class CloudController : MonoBehaviour, IWeather
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            // Debug.Log($"Clouds got {conditions.Rain}, {conditions.Wind}");
        }
    }
}
