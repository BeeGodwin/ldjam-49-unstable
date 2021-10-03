using UnityEngine;

namespace Bee.Sky
{
    public class DayNightController : MonoBehaviour
    {
        private int _hour;

        public float sunlightIntensity;
        public float moonlightIntensity;
        public float starlightIntensity;
        public float lanternIntensity;
        
        private CelestialBody _sun;
        private CelestialBody _moon;
        private Transform _easternHorizon;
        private Transform _westernHorizon;
        private Transform _zenith;
        
        private Light _sunLight;
        private Light _moonLight;
        private Light _starLight;
        private Light _lantern;

        public void Start()
        {
            _sun = GameObject.Find("Sun").GetComponent<CelestialBody>();
            _moon = GameObject.Find("Moon").GetComponent<CelestialBody>();
            _easternHorizon = GameObject.Find("EasternHorizon").transform;
            _westernHorizon = GameObject.Find("WesternHorizon").transform;
            _zenith = GameObject.Find("Zenith").transform;
            
            _sunLight = _sun.GetComponent<Light>();
            _moonLight = _moon.GetComponent<Light>();
            _starLight = GameObject.Find("Stars").GetComponent<Light>();
            _lantern = GameObject.Find("Lantern").GetComponent<Light>();
        }
        
        public void StartNextHour(float length)
        {
            if (_hour % 12 == 0)
            {
                StartSun(length * 6);
                HideMoon();
            }

            if (_hour == 6)
            {
                StartMoon(length * 6);
                HideSun();
            }
            _hour = (_hour + 1) % 12;
        }

        private void StartSun(float dayLength)
        {
            _sunLight.intensity = sunlightIntensity;
            _sun.StartJourney(_easternHorizon, _westernHorizon, _zenith, dayLength);
        }

        private void HideSun()
        {
            _sunLight.intensity = 0f;
            _sun.transform.position = _westernHorizon.position;
        }

        private void StartMoon(float nightLength)
        {
            _moonLight.intensity = moonlightIntensity;
            _lantern.intensity = lanternIntensity;
            _starLight.intensity = starlightIntensity;
            _moon.StartJourney(_easternHorizon, _westernHorizon, _zenith, nightLength);
        }

        private void HideMoon()
        {
            _moonLight.intensity = 0f;
            _starLight.intensity = 0f;
            _lantern.intensity = 0f;
            _moon.transform.position = _westernHorizon.position;
        }
    }
}
