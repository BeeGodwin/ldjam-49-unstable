using System.Collections.Generic;
using UnityEngine;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour, IWeather
    {
        public float wavePeriodFactor;
        public float waveLengthFactor;
        public float waveMagnitude;

        public float oceanExtent;
        public float nodeScale;
        public float nodeInterval;
        public GameObject oceanNodePrefab;
        private List<OceanNodeController> _nodes = new List<OceanNodeController>();
        
        public float yPos;

        public void Start()
        {
            InstantiateNodes();
        }

        public void Update()
        {
            var radians = (Time.time * wavePeriodFactor) % 360 * Mathf.Deg2Rad;
            
            _nodes.ForEach(node =>
            {
                var x = node.transform.position.x;
                var positionModifier = radians + x * waveLengthFactor;
                var sin = Mathf.Sin(positionModifier);
                var y = sin * waveMagnitude;
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
            Debug.Log($"Setting conditions to {conditions}");
        }
    }
}
