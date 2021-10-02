using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour
    {
        public float wavePeriodFactor;
        public float waveLengthFactor;
        public float waveMagnitude;

        private List<OceanNodeController> _nodes = new List<OceanNodeController>();

        private float _yPos;
        
        void Start()
        {
            _yPos = transform.GetChild(0).transform.position.y;
            foreach (Transform child in transform)
            {
                var node = child.GetComponent<OceanNodeController>();
                _nodes.Add(node);
            }
        }

        void Update()
        {
            var radians = (Time.time * wavePeriodFactor) % 360 * Mathf.Deg2Rad;
            
            _nodes.ForEach(node =>
            {
                var x = node.transform.position.x;
                var positionModifier = radians + x * waveLengthFactor;
                var sin = Mathf.Sin(positionModifier);
                var y = sin * waveMagnitude;
                node.transform.position = new Vector2(x, y + _yPos);
            });
        }
    }
}
