using System;
using System.Collections.Generic;
using System.Linq;
using Bee.Game;
using Bee.Sky;
using UnityEngine;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour, IWeather, IBlowable, IGameSystem
    {
        public float wavePeriodFactor;
        public float waveLengthFactor;
        public float maxWaveMagnitude;
        public float depthLevels;
        public float depthLevelWidth;
        
        private float _periodFactor;
        private float _lengthFactor;
        private float _magnitude;
        
        // private float _periodDelta;
        // private float _lengthDelta;
        private float _magnitudeDelta;
        private float _adjustTimer;
        private float _adjustTime;
        
        public float oceanExtent;
        public float nodeScale;
        public float nodeInterval;
        public GameObject oceanNodePrefab;

        private WeatherConditions _conditions;
        private List<OceanNodeController> _nodes = new List<OceanNodeController>();
        private LineRenderer _surfaceLine;
        private List<LineRenderer> _depthLines;
        
        public float yPos;

        public void Start()
        {
            _surfaceLine = GetComponentInChildren<LineRenderer>();
            InstantiateNodes();
            InstantiateDepthLines();
            _periodFactor = wavePeriodFactor;
            _lengthFactor = waveLengthFactor;
            _magnitude = 0.1f;
        }

        public void Update()
        {

            if (_adjustTimer > 0f)
            {
                _adjustTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / _adjustTime;
                // _periodFactor += _periodDelta * tDelta;
                // _lengthFactor += _lengthDelta * tDelta;
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

            DrawOcean();
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

        private void InstantiateDepthLines()
        {
            _depthLines = new List<LineRenderer>();
            for (int i = 0; i < depthLevels; i++)
            {
                _depthLines.Add(Instantiate(_surfaceLine, transform));
            }
        }

        private void DrawOcean()
        {
            var drawPositions = _nodes.ConvertAll(node => node.transform.GetChild(1).transform.position);

            var windFactor = WindFactor(_conditions);
            
            for (var i = _depthLines.Count; i > 0; i--)
            {
                var line = _depthLines[i - 1];
                line.positionCount = drawPositions.Count;
                var col = _surfaceLine.startColor;
                var t = 1f - i * 0.15f;
                var lineColor = new Color(col.r * t, col.b * t, col.g * t, 1f);
                line.startColor = lineColor;
                line.endColor = lineColor;
                line.widthMultiplier = depthLevelWidth * 2;
                var positions = drawPositions.ConvertAll(v3 => new Vector3(v3.x - windFactor * i, v3.y - i * depthLevelWidth, v3.z));
                line.SetPositions(positions.ToArray());
            }
            _surfaceLine.positionCount = drawPositions.Count;
            _surfaceLine.SetPositions(drawPositions.ToArray());
        }

        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            _adjustTime = time / 2;
            _adjustTimer = time / 2;
            _conditions = conditions;
            var rainFactor = RainFactor(conditions);
            var windFactor = WindFactor(conditions);
            
            var targetMagnitude = (rainFactor + windFactor) * maxWaveMagnitude;
            _magnitudeDelta = targetMagnitude - _magnitude;
            
            // Debug.Log($"DEBUG:WEATHER:OCEAN target magnitude; {targetMagnitude}, delta; {_magnitudeDelta}; current magnitude {_magnitude}. Transition over {_adjustTimer} secs.");
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

        public void ApplyWindForce(float force)
        {
            // throw new NotImplementedException();
        }

        public void PlayGame()
        {
            throw new NotImplementedException();
        }

        public void PauseGame()
        {
            throw new NotImplementedException();
        }

        public void ResetGame()
        {
            throw new NotImplementedException();
        }
    }
}
