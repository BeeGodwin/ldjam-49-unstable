using System.Timers;
using UnityEngine;

namespace Bee.Sky
{
    public class CelestialBody : MonoBehaviour
    {
        public AnimationCurve curve;

        private float _journeyTime;
        private float _journeyTimer;
        private bool _ascending;

        private bool _running;
        
        private Transform _start;
        private Transform _end;
        private Transform _setAt;

        public void StartJourney(Transform easternHorizon, Transform westernHorizon, Transform zenith, float time)
        {
            _running = true;
            _ascending = true;
            transform.position = easternHorizon.position;
            _start = easternHorizon;
            _end = zenith;
            _setAt = westernHorizon;
            _journeyTime = time / 2;
            _journeyTimer = time / 2;
            // Debug.Log($"{gameObject.name} rose at {_start} and is ascending");
        }

        public void Update()
        {
            if (!_running) return; // cheesy hack to prevent this running on 1st frame and breaking for some reason
            
            if (_journeyTimer > 0)
            {
                _journeyTimer -= Time.deltaTime;
            }

            if (_journeyTimer <= 0f && _ascending)
            {
                _ascending = false;
                _journeyTimer = _journeyTime;
                transform.position = _end.position;
                _start = _end;
                _end = _setAt;
                // Debug.Log($"{gameObject.name} has reached {_start} and is descending");
            }

            if (_journeyTimer <= 0f)
            {
                transform.position = _end.position;
                // Debug.Log($"{gameObject.name} has set at {_end}");
                _running = false;
            }

            if (_running)
            {
                var t = 1 - _journeyTimer / _journeyTime;
                var yT = _ascending ? curve.Evaluate(t) : curve.Evaluate(1 - t);
                var y = _ascending ? _end.position.y * yT + _start.position.y * (1 - yT) : _end.position.y * (1 - yT) + _start.position.y * yT;
                var x = _end.position.x * t + _start.position.x * (1 - t);
                transform.position = new Vector3(x, y, 0);
            }
        }
    }
}
