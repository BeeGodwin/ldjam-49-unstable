using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour, IWeather
    {
        public float wavePeriodFactor;
        public float waveLengthFactor;
        public float maxWaveMagnitude;
        public float adjustTime;

        private float _periodFactor;
        private float _lengthFactor;
        private float _magnitude;

        private float _periodDelta;
        private float _lengthDelta;
        private float _magnitudeDelta;
        private float _adjustTimer;

        public float oceanExtent;
        public float nodeScale;
        public float nodeInterval;
        public GameObject oceanNodePrefab;
        private List<OceanNodeController> _nodes = new List<OceanNodeController>();
        
        public float yPos;

        public void Start()
        {
            InstantiateNodes();
            _periodFactor = wavePeriodFactor;
            _lengthFactor = waveLengthFactor;
            _magnitude = 0.1f;
        }

        public void Update()
        {

            if (_adjustTimer > 0f)
            {
                _adjustTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / adjustTime;
                _periodFactor += _periodDelta * tDelta;
                _lengthFactor += _lengthDelta * tDelta;
                _magnitude += _magnitudeDelta * tDelta;
            }

            var radians = (Time.time * _periodFactor) % 360 * Mathf.Deg2Rad;

            _nodes.ForEach(node =>
            {
                var x = node.transform.position.x;
                var positionModifier = radians + x * _lengthFactor;
                var sin = Mathf.Sin(positionModifier);
                var y = sin * _magnitude;
                node.transform.position = new Vector2(x, y + yPos);
            });
        }

        private void InstantiateNodes()
        {
            var xPos = -oceanExtent;
            while (xPos <= oceanExtent)
            {
                var go = GameObject.Instantiate(oceanNodePrefab, transform);
                go.transform.Translate(Vector2.down * yPos + Vector2.right * xPos);
                go.transform.localScale = Vector3.one * nodeScale;
                _nodes.Add(go.GetComponent<OceanNodeController>());
                xPos += nodeInterval;
            }
        }

        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            // TODO: maybe make adjust time based on time
            
            _adjustTimer = adjustTime;
            var rainFactor = RainFactor(conditions);
            var windFactor = WindFactor(conditions);
            
            var targetMagnitude = (rainFactor + windFactor) * maxWaveMagnitude;
            _magnitudeDelta = targetMagnitude - _magnitude;
            
            Debug.Log($"DEBUG:WEATHER:OCEAN target magnitude; {targetMagnitude}, delta; {_magnitudeDelta}; current magnitude {_magnitude}. Transition over {_adjustTimer} secs.");
        }

        private float RainFactor(WeatherConditions conditions)
        {
            float value;
            switch (conditions.Rain)
            {
                case Rain.LightRain:
                    value = 0.1f;
                    break;
                case Rain.HardRain:
                    value = 0.2f;
                    break;
                case Rain.TorrentialRain:
                    value = 0.4f;
                    break;
                default:
                    value = 0f;
                    break;
            }
            return value;
        }

        private float WindFactor(WeatherConditions conditions)
        {
            float value;
            switch (conditions.Wind)
            {
                case Wind.Breezy:
                    value = 0.2f;
                    break;
                case Wind.Windy:
                    value = 0.4f;
                    break;
                case Wind.Gale:
                    value = 0.6f;
                    break;
                default:
                    value = 0.1f;
                    break;
            }
            return value;
        }
    }
}
