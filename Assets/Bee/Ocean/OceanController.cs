using System.Collections.Generic;
using UnityEngine;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour, IWeather
    {
        public float wavePeriodFactor;
        public float waveLengthFactor;
        public float waveMagnitude;

        private float _periodFactor;
        private float _lengthFactor;
        private float _magnitude;

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
            _magnitude = waveMagnitude;
        }

        public void Update()
        {
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

        public void SetWeatherConditions(WeatherConditions conditions)
        {
            // Debug.Log($"Setting conditions to {conditions}");

            if (conditions == WeatherConditions.MildSwell)
            {
                _periodFactor = wavePeriodFactor;
                _lengthFactor = waveLengthFactor;
                _magnitude = waveMagnitude;
            } else if (conditions == WeatherConditions.BigSwell)
            {
                _periodFactor = (float) (wavePeriodFactor * 1.25);
                _lengthFactor = (float) (waveLengthFactor * 1.25);
                _magnitude = waveMagnitude * 2;
            }
            else
            {
                _periodFactor = (float) (wavePeriodFactor / 1.25);
                _lengthFactor = (float) (waveLengthFactor / 1.25);
                _magnitude = waveMagnitude / 2;
            }
        }
    }
}
