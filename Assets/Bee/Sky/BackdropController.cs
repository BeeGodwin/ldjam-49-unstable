using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bee.Sky
{
    public class BackdropController : MonoBehaviour, IWeather
    {
        private int _hourCount = -1;
    
        public Sprite _bg_00_05;
        public Sprite _bg_06_11;
        public Sprite _bg_12_17;
        public Sprite _bg_18_23;

        public float startY;
        public float endY;
        
        private List<Sprite> _backgroundSprites;
        private SpriteRenderer _rendA;
        private SpriteRenderer _rendB;
        private SpriteRenderer _activeRend;

        private float _moveTime;
        private float _moveTimer;

        private float _fadeTime;
        private float _fadeTimer;

        private bool _running;
    
        // Start is called before the first frame update
        void Start()
        {
            _backgroundSprites = new List<Sprite> { _bg_06_11, _bg_12_17, _bg_18_23, _bg_00_05 };
            _rendA = GameObject.Find("RendA").GetComponent<SpriteRenderer>();
            _rendB = GameObject.Find("RendB").GetComponent<SpriteRenderer>();
            _running = false;
        }

        void Update()
        {
            if (!_running) return;
            
            if (_moveTimer > 0)
            {
                _moveTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / _moveTime;
                var range = endY - startY;
                var spriteTransform = _activeRend.transform;
                var spritePosition = spriteTransform.position;
                spriteTransform.position = new Vector3(spritePosition.x, spritePosition.y + tDelta * range, spritePosition.z);
            }

            if (_fadeTimer > 0)
            {
                _fadeTimer -= Time.deltaTime;
                var tDelta = Time.deltaTime / _fadeTime;
                _activeRend.color = new Color(1f, 1f, 1f, Mathf.Min(_activeRend.color.a + tDelta, 1f));
                if (_activeRend == _rendA)
                {
                    _rendB.color = new Color(1f, 1f, 1f, _rendB.color.a - tDelta);
                }
                else
                {
                    _rendA.color = new Color(1f, 1f, 1f, _rendA.color.a - tDelta);
                }
            }
        }


        public void SetWeatherConditions(WeatherConditions conditions, float time)
        {
            _hourCount = (_hourCount + 1) % 12;
            
            if (_hourCount % 3 == 0)
            {
                var backgroundIndex = (int)Math.Floor((float)_hourCount / 3);
                _activeRend = _activeRend == _rendA ? _rendB : _rendA;
                _activeRend.sprite = _backgroundSprites[backgroundIndex];
                var spriteTransform = _activeRend.transform;
                var spritePosition = spriteTransform.position;
                spriteTransform.position = new Vector3(spritePosition.x, startY, spritePosition.z);
                
                _fadeTime = time / 2;
                _fadeTimer = time / 2;
                _moveTime = time * 3;
                _moveTimer = time * 3;
            }
        }

        public void Stop()
        {
            
        }

        public void Go()
        {
            
        }
    }
}
