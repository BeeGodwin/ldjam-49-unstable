using System.Collections.Generic;
using Bee.Ocean;
using Bee.Raft;
using UnityEngine;

namespace Bee.Sky
{
    public class WindController : MonoBehaviour
    {
        public float maxWindForce;
        
        private Wind _wind;

        private float _windForce;
        private float _windForceDelta;

        private float _transitionTimer;
        private float _transitionTime;

        private List<IBlowable> _blowables;
        
        void Start()
        {
            _blowables = new List<IBlowable>();
            _blowables.Add((IBlowable) GameObject.Find("Raft").GetComponentInChildren<RaftController>());
            var ocean = GameObject.Find("Ocean").GetComponentInChildren<OceanController>();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (_transitionTimer > 0)
            {
                _transitionTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / _transitionTime;

                _windForce = _windForce + tDelta * _windForceDelta;
            }

            _blowables.ForEach(blowable => blowable.ApplyWindForce(_windForce * Time.deltaTime));
        }

        public void SetWindConditions(Wind wind, float time)
        {
            _transitionTime = time / 2;
            _transitionTimer = time / 2;

            _wind = wind;

            switch (_wind)
            {
                case Wind.None:
                    _windForceDelta = 0.1f * maxWindForce - _windForce;
                    break;
                case Wind.Breezy:
                    _windForceDelta = 0.3f * maxWindForce - _windForce;
                    break;
                case Wind.Windy:
                    _windForceDelta = 0.6f * maxWindForce - _windForce;
                    break;
                case Wind.Gale:
                    _windForceDelta = 1f * maxWindForce - _windForce;
                    break;
            }
        }
    }

    interface IBlowable
    {
        void ApplyWindForce(float force);
    }
}
