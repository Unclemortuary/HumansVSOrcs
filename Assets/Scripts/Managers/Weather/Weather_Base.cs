using UnityEngine;
using System.Collections;

using Project.TimeManager;


namespace Project.Weather
{
    public class Weather_Base : MonoBehaviour
    {
        [SerializeField]
        protected bool _isUseDifferentFadeTimes;

        [SerializeField]
        protected float _fadeTime = 5.0f;

        [SerializeField]
        protected float _sunriseFadeTime = 5.0f;

        [SerializeField]
        protected float _dayFadeTime = 5.0f;

        [SerializeField]
        protected float _sunsetFadeTime = 5.0f;

        [SerializeField]
        protected float _nightFadeTime = 5.0f;

        [SerializeField]
        protected bool _isUseInit;

        protected float _initTimerStart = 0.0f;

        [SerializeField]
        protected float _initTimerEnd = 5.0f;


        /********** TEMPRATURE Settings **********/

        [SerializeField]
        protected float _minTemperature = 0.0f;

        [SerializeField]
        protected float _maxTemperature = 30.0f;

        /********** SUN (Light) Settings **********/

        // LIGHT INTENSITY
        [SerializeField]
        protected float _sunriseLightIntensity = 0.5f;

        [SerializeField]
        protected float _dayLightIntensity = 1.0f;

        [SerializeField]
        protected float _sunsetLightIntensity = 0.5f;

        [SerializeField]
        protected float _nightLightIntensity = 0.3f;

        // LIGHT COLOR
        [SerializeField]
        protected Color _sunriseLightColor = Color.yellow;

        [SerializeField]
        protected Color _dayLightColor = Color.cyan;

        [SerializeField]
        protected Color _sunsetLightColor = Color.red;

        [SerializeField]
        protected Color _nightLightColor = Color.grey;

        /********** MOON (Light) Settings **********/

        [SerializeField]
        protected float _nightMoonLightIntensity = 0.5f;

        [SerializeField]
        protected Color _nightMoonLightColor = Color.grey;

        /********** SKYBOX Settings **********/

        // Skybox
        [SerializeField]
        protected Color _sunriseSkyColor = Color.yellow;

        [SerializeField]
        protected Color _daySkyColor = Color.cyan;

        [SerializeField]
        protected Color _sunsetSkyColor = Color.red;

        [SerializeField]
        protected Color _nightSkyColor = Color.grey;

        /********** CLOUD Settings **********/

        [SerializeField]
        protected Color _cloudColor = Color.white;

        /********** FOG Settings **********/

        [SerializeField]
        protected float _fogAmount = 0.05f;

        [SerializeField]
        protected float _fogMorningAmount = 0.05f;

        [SerializeField]
        protected bool _isUseMorningFog = false;

        [SerializeField]
        protected Color _fogColor = Color.white;

        /********** GENERAL **********/

        public bool IsUseDifferentFadeTimes
        {
            get { return _isUseDifferentFadeTimes; }
            set { _isUseDifferentFadeTimes = value; }
        }

        public float FadeTime
        {
            get { return _fadeTime; }
            set { _fadeTime = value; }
        }

        public float SunriseFadeTime
        {
            get { return _sunriseFadeTime; }
            set { _sunriseFadeTime = value; }
        }

        public float DayFadeTime
        {
            get { return _dayFadeTime; }
            set { _dayFadeTime = value; }
        }

        public float SunsetFadeTime
        {
            get { return _sunsetFadeTime; }
            set { _sunsetFadeTime = value; }
        }

        public float NightFadeTime
        {
            get { return _nightFadeTime; }
            set { _nightFadeTime = value; }
        }

        public bool IsUseInit
        {
            get { return _isUseInit; }
            set { _isUseInit = value; }
        }

        public float InitTimerEnd
        {
            get { return _initTimerEnd; }
            set { _initTimerEnd = value; }
        }

        /********** TEMPRATURE **********/

        public float MinTemperature
        {
            get { return _minTemperature; }
            set { _minTemperature = value; }
        }

        public float MaxTemperature
        {
            get { return _maxTemperature; }
            set { _maxTemperature = value; }
        }

        /********** SUN (Light) **********/

        public float SunriseLightIntensity
        {
            get { return _sunriseLightIntensity; }
            set { _sunriseLightIntensity = value; }
        }

        public float DayLightIntensity
        {
            get { return _dayLightIntensity; }
            set { _dayLightIntensity = value; }
        }

        public float SunsetLightIntensity
        {
            get { return _sunsetLightIntensity; }
            set { _sunsetLightIntensity = value; }
        }

        public float NightLightIntensity
        {
            get { return _nightLightIntensity; }
            set { _nightLightIntensity = value; }
        }

        public Color SunriseLightColor
        {
            get { return _sunriseLightColor; }
            set { _sunriseLightColor = value; }
        }

        public Color DayLightColor
        {
            get { return _dayLightColor; }
            set { _dayLightColor = value; }
        }

        public Color SunsetLightColor
        {
            get { return _sunsetLightColor; }
            set { _sunsetLightColor = value; }
        }

        public Color NightLightColor
        {
            get { return _nightLightColor; }
            set { _nightLightColor = value; }
        }

        /********** MOON (Light) **********/

        public float NightMoonLightIntensity
        {
            get { return _nightMoonLightIntensity; }
            set { _nightMoonLightIntensity = value; }
        }

        public Color NightMoonLightColor
        {
            get { return _nightMoonLightColor; }
            set { _nightMoonLightColor = value; }
        }

        /********** SKYBOX **********/

        public Color SunriseSkyColor
        {
            get { return _sunriseSkyColor; }
            set { _sunriseSkyColor = value; }
        }

        public Color DaySkyColor
        {
            get { return _daySkyColor; }
            set { _daySkyColor = value; }
        }

        public Color SunsetSkyColor
        {
            get { return _sunsetSkyColor; }
            set { _sunsetSkyColor = value; }
        }

        public Color NightSkyColor
        {
            get { return _nightSkyColor; }
            set { _nightSkyColor = value; }
        }


        /********** CLOUD **********/

        public Color CloudColor
        {
            get { return _cloudColor; }
            set { _cloudColor = value; }
        }

        /********** FOG **********/

        public float FogAmount
        {
            get { return _fogAmount; }
            set { _fogAmount = value; }
        }

        public float FogMorning
        {
            get { return _fogMorningAmount; }
            set { _fogMorningAmount = value; }
        }

        public bool IsUseMorningFog
        {
            get { return _isUseMorningFog; }
            set { _isUseMorningFog = value; }
        }

        public Color FogColor
        {
            get { return _fogColor; }
            set { _fogColor = value; }
        }

        /********** ----- FUNCTIONS ----- **********/

        public virtual void Init()
        {
            float currentTemprature = Random.Range(_minTemperature, _maxTemperature);
            WeatherManager.GetInstance.CurrTemperature = currentTemprature;
        }

        internal void UpdateWeather() {
            if (!_isUseDifferentFadeTimes) {
                OneFadeTimeToRuleThemAll();
            } else {
                DifferentFadeTimes();
            }
        }

        public virtual void ExitWeatherEffect() {}

        internal void OneFadeTimeToRuleThemAll() {
            switch (TimeManager.TimeManager.GetInstance.CurrTimeOfDay) {
                case TimeOfDay.SUNRISE:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _sunriseLightIntensity, _sunriseLightColor, 0.0f, _nightMoonLightColor,
                        _sunriseSkyColor, _cloudColor, _fogMorningAmount, _fogColor, _fadeTime
                    );
                    break;

                case TimeOfDay.DAY:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _dayLightIntensity, _dayLightColor, 0.0f, _nightMoonLightColor,
                        _daySkyColor, _cloudColor, _fogAmount, _fogColor, _fadeTime
                    );
                    break;

                case TimeOfDay.NIGHT:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _nightLightIntensity, _nightLightColor, _nightMoonLightIntensity,
                        _nightMoonLightColor, _nightSkyColor, _cloudColor, _fogAmount, _fogColor, _fadeTime
                    );
                    break;
            }
        }

        internal void DifferentFadeTimes() {
            switch (TimeManager.TimeManager.GetInstance.CurrTimeOfDay) {
                case TimeOfDay.SUNRISE:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _sunriseLightIntensity, _sunriseLightColor, 0.0f, _nightMoonLightColor,
                        _sunriseSkyColor, _cloudColor, _fogMorningAmount, _fogColor, _sunriseFadeTime
                    );
                    break;

                case TimeOfDay.DAY:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _dayLightIntensity, _dayLightColor, 0.0f, _nightMoonLightColor,
                        _daySkyColor, _cloudColor, _fogAmount, _fogColor, _dayFadeTime
                    );
                    break;

                case TimeOfDay.SUNSET:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _sunsetLightIntensity, _sunsetLightColor, 0.0f, _nightMoonLightColor,
                        _sunsetSkyColor, _cloudColor, _fogAmount, _fogColor, _sunsetFadeTime
                    );
                    break;

                case TimeOfDay.NIGHT:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _nightLightIntensity, _nightLightColor, _nightMoonLightIntensity, _nightMoonLightColor,
                        _nightSkyColor, _cloudColor, _fogAmount, _fogColor, _nightFadeTime
                    );
                    break;
            }
        }
    }
}