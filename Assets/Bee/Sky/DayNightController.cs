using UnityEngine;

namespace Bee.Sky
{
    public class DayNightController : MonoBehaviour
    {
        private int _hour;
        private bool _isDay;

        public float sunlightIntensity;
        public float moonlightIntensity;
        public float starlightIntensity;
        public float lanternIntensity;
        
        private Transform _sun;
        private Transform _moon;
        private Light _sunLight;
        private Light _moonLight;
        private Light _starLight;
        private Light _lantern;

        public void Start()
        {
            _sun = GameObject.Find("Sun").transform;
            _moon = GameObject.Find("Moon").transform;
            _sunLight = _sun.GetComponent<Light>();
            _moonLight = _moon.GetComponent<Light>();
            _starLight = GameObject.Find("Stars").GetComponent<Light>();
            _lantern = GameObject.Find("Lantern").GetComponent<Light>();
        }

        public void StartDay()
        {
            _hour = 0;
            StartSun();
            HideMoon();
        }

        public void StartNextHour(float length)
        {
            _hour++;
            if (_hour == 6)
            {
                StartMoon();
                HideSun();
            }
        }

        private void StartSun()
        {
        }

        private void HideSun()
        {
        }

        private void StartMoon()
        {
        }

        private void HideMoon()
        {
        }
    }
}
