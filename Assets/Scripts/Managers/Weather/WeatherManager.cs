using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.TimeManager;


namespace Project.Weather
{
    public class WeatherManager : MonoBehaviour
    {
        private static WeatherManager _instance;
        public static WeatherManager GetInstance
        {
            get
            {
                return _instance;
            }
        }

        private bool _isUseRandomWeather = true;
        private bool _isUseRandomTimeWeather = true;
        private int _minTimeBeforeChanging = 1000;
        private int _maxTimeBeforeChanging = 1500;
        private int _changeAfterTime = 13;

        [SerializeField]
        private WeatherType _currWeather;
        public WeatherType CurrWeather
        {
            get { return _currWeather; }
        }

        [SerializeField]
        public WeatherType _currDebugWeather;
        private WeatherType _lastWeather;
        private int _newWeather;

        private float _startChanging;
        private float _endChanging;
        private bool _isChanging;
        private bool _canChange;
        [SerializeField]
        private float _timeSinceLastWeather;
        [SerializeField]
        private float _timeToNewWeather;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _startChanging = 0.0f;
            _endChanging = 1.0f;

            if (_isUseRandomTimeWeather)
            {
                _timeToNewWeather = Random.Range(_minTimeBeforeChanging, _maxTimeBeforeChanging);
            }
            else
            {
                _timeToNewWeather = _changeAfterTime;
            }

            if (_currWeather == WeatherType.RANDOM
                || _currWeather == WeatherType.NUMBEROFWEATHERTYPES)
            {
                if (_isUseRandomTimeWeather)
                {
                    PickRandomWeather();
                }
                else
                {
                    Debug.LogWarning("You haven't picked which weather to use, we default to SUN if the weather type is still set to \"RANDOM or NUMBEROFWEATHERTYPES\" and \"Use random weather\" is off");
                    ExitCurrentWeather((int)WeatherType.SUN);
                }
            }
            else
            {
                EnterNewWeather((int)_currWeather);
            }
        }

        private void PickRandomWeather()
        {
            int randomWeather;
            randomWeather = Random.Range(1, (int)WeatherType.NUMBEROFWEATHERTYPES);

            if (randomWeather != (int)_currWeather)
            {
                _newWeather = randomWeather;
                _isChanging = true;
            }
            else
            {
                Debug.Log("We got the same weather no change will happen!");
            }

            _canChange = false;
        }

        void ExitCurrentWeather(int newWeather)
        {
            _lastWeather = _currWeather;
            if (_currWeather == WeatherType.RANDOM)
            {
                EnterNewWeather(newWeather);
                _startChanging = 0.0f;
                _isChanging = false;
            }
            else
            {
                _startChanging += Time.deltaTime;
                if (_startChanging >= _endChanging)
                {
                    switch (_currWeather)
                    {
                        case WeatherType.SUN:
                            this.GetComponent<Weather_Sun>().ExitWeatherEffect();
                            this.GetComponent<Weather_Sun>().enabled = false;
                            break;

                        case WeatherType.RAIN:
                            this.GetComponent<Weather_Rain>().ExitWeatherEffect();
                            this.GetComponent<Weather_Rain>().enabled = false;
                            break;

                        case WeatherType.SNOW:
                            this.GetComponent<Weather_Snow>().ExitWeatherEffect();
                            this.GetComponent<Weather_Snow>().enabled = false;
                            break;
                    }

                    EnterNewWeather(newWeather);
                    _startChanging = 0.0f;
                    _isChanging = false;
                }
            }
        }

        private void EnterNewWeather(int newWeather) {
            switch (newWeather) {
                case (int)WeatherType.SUN:
                    ChangeWeatherToSun();
                    break;

                case (int)WeatherType.RAIN:
                    ChangeWeatherToRain();
                    break;

                case (int)WeatherType.SNOW:
                    ChangeWeatherToSnow();
                    break;
            }
        }

        public void ChangeWeatherToSun(){
            _currWeather = WeatherType.SUN;
            _currDebugWeather = WeatherType.SUN;
            this.GetComponent<Weather_Sun>().enabled = true;
            this.GetComponent<Weather_Sun>().IsUseInit = true;
        }

        public void ChangeWeatherToRain() {
            _currWeather = WeatherType.RAIN;
            _currDebugWeather = WeatherType.RAIN;
            this.GetComponent<Weather_Rain>().enabled = true;
            this.GetComponent<Weather_Rain>().IsUseInit = true;
        }

        public void ChangeWeatherToSnow() {
            _currWeather = WeatherType.SNOW;
            _currDebugWeather = WeatherType.SNOW;
            this.GetComponent<Weather_Snow>().enabled = true;
            this.GetComponent<Weather_Snow>().IsUseInit = true;
        }

        public void SetWeather(WeatherType newWeather) {
            _timeSinceLastWeather = 0;
            _canChange = false;
            _timeToNewWeather = 10;
            _newWeather = (int)newWeather;
            _isChanging = true;
            ExitCurrentWeather(_newWeather);
        }

        void Update() {
            if (_currDebugWeather != _currWeather)
            {
                _newWeather = (int)_currDebugWeather;
                ExitCurrentWeather(_newWeather);
                _timeSinceLastWeather = 0;
            }
            _timeSinceLastWeather += Time.deltaTime;
            if (_isUseRandomWeather) {
                if (_timeSinceLastWeather >= _timeToNewWeather) {
                    _timeSinceLastWeather = 0;
                    _canChange = true;
                    if (_isUseRandomTimeWeather) {
                        _timeToNewWeather = Random.Range(_minTimeBeforeChanging
                            , _maxTimeBeforeChanging);
                    }
                }
            }

            if (_canChange) {
                PickRandomWeather();
            }

            if (_isChanging == true) {
                ExitCurrentWeather(_newWeather);
            }
        }


        public void UpdateAllWeather(
            float sunIntensity
            , Color sunLightColor
            , Color sky
            , float fogDensity
            , Color fogColor
            , float fadeTime)
        {

            TimeManager.TimeManager.GetInstance.sunLight.intensity =
                Mathf.Lerp(
                    TimeManager.TimeManager.GetInstance.sunLight.intensity
                    , sunIntensity
                    , Time.deltaTime / fadeTime
                );

            TimeManager.TimeManager.GetInstance.sunLight.color =
                Color.Lerp(
                    TimeManager.TimeManager.GetInstance.sunLight.color
                    , sunLightColor
                    , Time.deltaTime / fadeTime
                );


            // Skybox settings
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(RenderSettings.skybox.GetColor("_Tint"), sky, Time.deltaTime / fadeTime));

            // Fog settings
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDensity, Time.deltaTime / fadeTime);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColor, Time.deltaTime / fadeTime);

            DynamicGI.UpdateEnvironment();
        }
    }
}