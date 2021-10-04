using UnityEngine;

namespace Bee.Sky
{
    public class RainController : MonoBehaviour
    {
        private Rain _rain;
        private Wind _wind;

        private float _roTDelta;
        private float _angleDelta;
        private float _transitionTimer = 0;
        private float _transitionTime = 0;
        
        private ParticleSystem _particle;
        
        void Start()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (_transitionTimer > 0)
            {
                _transitionTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / _transitionTime;
                
                var emission = _particle.emission;
                var roT = emission.rateOverTime.constant;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(roT + tDelta * _roTDelta);
                
                var shape = _particle.shape;
                var angle = shape.rotation.z;
                shape.rotation = new Vector3(0, 0, angle + tDelta * _angleDelta);
            }
        }

        public void SetRainConditions(WeatherConditions conditions, float time)
        {
            _transitionTime = time / 2;
            _transitionTimer = time / 2;

            _rain = conditions.Rain;
            _wind = conditions.Wind;

            var emission = _particle.emission;
            switch (_rain)
            {
                case Rain.None:
                    _roTDelta = 0 - emission.rateOverTime.constant;
                    break;
                case Rain.LightRain:
                    _roTDelta = 10 - emission.rateOverTime.constant;
                    break;
                case Rain.HardRain:
                    _roTDelta = 30 - emission.rateOverTime.constant;
                    break;
                case Rain.TorrentialRain:
                    _roTDelta = 50 - emission.rateOverTime.constant;
                    break;
            }

            var shape = _particle.shape;
            switch (_wind)
            {
                case Wind.None:
                    _angleDelta = 0 - shape.rotation.z;
                    break;
                case Wind.Breezy:
                    _angleDelta = 10 - shape.rotation.z;
                    break;
                case Wind.Windy:
                    _angleDelta = 30 - shape.rotation.z;
                    break;
                case Wind.Gale:
                    _angleDelta = 45 - shape.rotation.z;
                    break;
            }
            // Debug.Log($"DEBUG:WEATHER:RAIN: {_rain} and {_wind}. angleDelta: {_angleDelta}. RoTDelta: {_roTDelta}.");
        }
        public void Stop()
        {
            _particle.Pause();
        }

        public void Go()
        {
            _particle.Play();
        }
    }
}
