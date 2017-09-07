using UnityEngine;
using System.Collections;

using Project.TimeManager;


namespace Project.Weather
{
    public class Weather_Base : MonoBehaviour
    {
        protected bool _isUseDifferentFadeTimes;

        [SerializeField]
        protected float _fadeTime = 3.0f;

        protected float _sunriseFadeTime = 3.0f;
        protected float _dayFadeTime = 3.0f;
        protected float _sunsetFadeTime = 3.0f;
        protected float _nightFadeTime = 3.0f;

        protected bool _isUseInit;

        protected float _initTimerStart = 0.0f;
        protected float _initTimerEnd = 3.0f;

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

        /********** FOG Settings **********/

        [SerializeField]
        protected float _fogAmount = 0.005f;

        [SerializeField]
        protected float _fogMorningAmount = 0.005f;

        [SerializeField]
        protected bool _isUseMorningFog = false;

        [SerializeField]
        protected Color _fogColor = Color.white;

		[SerializeField]
		protected Color _ambientColorDay = Color.white;
		[SerializeField]
		protected Color _ambientColorNight= Color.blue;

        /********** GENERAL **********/

        public bool IsUseDifferentFadeTimes {
            get { return _isUseDifferentFadeTimes; }
            set { _isUseDifferentFadeTimes = value; }
        }

        public float FadeTime {
            get { return _fadeTime; }
            set { _fadeTime = value; }
        }

        public float SunriseFadeTime {
            get { return _sunriseFadeTime; }
            set { _sunriseFadeTime = value; }
        }

        public float DayFadeTime {
            get { return _dayFadeTime; }
            set { _dayFadeTime = value; }
        }

        public float SunsetFadeTime {
            get { return _sunsetFadeTime; }
            set { _sunsetFadeTime = value; }
        }

        public float NightFadeTime {
            get { return _nightFadeTime; }
            set { _nightFadeTime = value; }
        }

        public bool IsUseInit {
            get { return _isUseInit; }
            set { _isUseInit = value; }
        }

        public float InitTimerEnd {
            get { return _initTimerEnd; }
            set { _initTimerEnd = value; }
        }

        /********** SUN (Light) **********/

        public float SunriseLightIntensity {
            get { return _sunriseLightIntensity; }
            set { _sunriseLightIntensity = value; }
        }

        public float DayLightIntensity {
            get { return _dayLightIntensity; }
            set { _dayLightIntensity = value; }
        }

        public float SunsetLightIntensity {
            get { return _sunsetLightIntensity; }
            set { _sunsetLightIntensity = value; }
        }

        public float NightLightIntensity {
            get { return _nightLightIntensity; }
            set { _nightLightIntensity = value; }
        }

        public Color SunriseLightColor {
            get { return _sunriseLightColor; }
            set { _sunriseLightColor = value; }
        }

        public Color DayLightColor {
            get { return _dayLightColor; }
            set { _dayLightColor = value; }
        }

        public Color SunsetLightColor {
            get { return _sunsetLightColor; }
            set { _sunsetLightColor = value; }
        }

        public Color NightLightColor {
            get { return _nightLightColor; }
            set { _nightLightColor = value; }
        }

        /********** SKYBOX **********/

        public Color SunriseSkyColor {
            get { return _sunriseSkyColor; }
            set { _sunriseSkyColor = value; }
        }

        public Color DaySkyColor {
            get { return _daySkyColor; }
            set { _daySkyColor = value; }
        }

        public Color SunsetSkyColor {
            get { return _sunsetSkyColor; }
            set { _sunsetSkyColor = value; }
        }

        public Color NightSkyColor {
            get { return _nightSkyColor; }
            set { _nightSkyColor = value; }
        }


        /********** FOG **********/

        public float FogAmount {
            get { return _fogAmount; }
            set { _fogAmount = value; }
        }

        public float FogMorning {
            get { return _fogMorningAmount; }
            set { _fogMorningAmount = value; }
        }

        public bool IsUseMorningFog {
            get { return _isUseMorningFog; }
            set { _isUseMorningFog = value; }
        }

        public Color FogColor {
            get { return _fogColor; }
            set { _fogColor = value; }
        }

        /********** ----- FUNCTIONS ----- **********/

        public virtual void Init() {}

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
                        _sunriseLightIntensity, _sunriseLightColor,
					_sunriseSkyColor, _fogMorningAmount, _fogColor, _fadeTime,_ambientColorDay
                    );
                    break;

                case TimeOfDay.DAY:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _dayLightIntensity, _dayLightColor,
					_daySkyColor, _fogAmount, _fogColor, _fadeTime,_ambientColorDay
                    );
                    break;

                case TimeOfDay.NIGHT:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _nightLightIntensity, _nightLightColor,
					_nightSkyColor, _fogAmount, _fogColor, _fadeTime,_ambientColorNight
                    );
                    break;
            }
        }

        internal void DifferentFadeTimes() {
            switch (TimeManager.TimeManager.GetInstance.CurrTimeOfDay) {
                case TimeOfDay.SUNRISE:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _sunriseLightIntensity, _sunriseLightColor, 
					_sunriseSkyColor, _fogMorningAmount, _fogColor, _sunriseFadeTime,_ambientColorDay
                    );
                    break;

                case TimeOfDay.DAY:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _dayLightIntensity, _dayLightColor, 
					_daySkyColor, _fogAmount, _fogColor, _dayFadeTime,_ambientColorDay
                    );
                    break;

                case TimeOfDay.SUNSET:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _sunsetLightIntensity, _sunsetLightColor,
					_sunsetSkyColor, _fogAmount, _fogColor, _sunsetFadeTime,_ambientColorDay
                    );
                    break;

                case TimeOfDay.NIGHT:
                    WeatherManager.GetInstance.UpdateAllWeather(
                        _nightLightIntensity, _nightLightColor,
					_nightSkyColor, _fogAmount, _fogColor, _nightFadeTime, _ambientColorNight
                    );
                    break;
            }
        }
    }
}