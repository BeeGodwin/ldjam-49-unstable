using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bee.Ocean
{
    public class OceanController : MonoBehaviour
    {
        public float wavelengthFactor;
        public float waveMagnitude;
        
        private List<OceanNodeController> _nodes = new List<OceanNodeController>();
        private float _waveTimer = 0;
    
        void Start()
        {
            foreach (Transform child in transform)
            {
                var node = child.GetComponent<OceanNodeController>();
                _nodes.Add(node);
            }
        }

        void Update()
        {
            _waveTimer += Time.deltaTime;
            _nodes.ForEach(node =>
            {
                var position = node.transform.position;
                var positionModifier = (position.x + 9) * wavelengthFactor + _waveTimer;
                var sin = Mathf.Sin(positionModifier);
                var y = position.y + sin * waveMagnitude;
                node.transform.position = new Vector2(position.x, y);
            });
        }
    }
}
