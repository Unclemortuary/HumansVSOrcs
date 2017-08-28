using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.Weather
{
    public class WeatherManager : MonoBehaviour
    {
        private static WeatherManager _instance;
        public static WeatherManager GetInstance {
            get {
                return _instance;
            }
        }


        private bool _isUseRandomDaysWeather = true;
        private int _minDaysBeforeChanging = 1;
        private int _maxDaysBeforeChanging = 10;
        private int _changeAfterDays = 4;


        private WeatherType _currWeather;
        public WeatherType CurrWeather {
            get { return _currWeather; }
        }
        private WeatherType _lastWeather;

        private float _startChanging;
        private float _endChanging;
        private bool _isChanging;
        private bool _canChange;

        private int _daysBeforeChanging;

        void Start() {
            _startChanging = 0.0f;
            _endChanging = 5.0f;

            if (_isUseRandomDaysWeather) {
                _daysBeforeChanging = Random.Range(_minDaysBeforeChanging, _maxDaysBeforeChanging);
            } else {
                _daysBeforeChanging = _changeAfterDays;
            }

            if (_currWeather == WeatherType.RANDOM 
                || _currWeather == WeatherType.NUMBEROFWEATHERTYPES) {
                if (_isUseRandomDaysWeather) {
                    PickRandomWeather();
                } else {
                    Debug.LogWarning("You haven't picked which weather to use, we default to SUN if the weather type is still set to \"RANDOM or NUMBEROFWEATHERTYPES\" and \"Use random weather\" is off");
                    ExitCurrentWeather((int)WeatherType.SUN);
                }
            } else {
                EnterNewWeather((int)_currWeather);
            }
        }

        private void PickRandomWeather() {
            int randomWeather;
            randomWeather = Random.Range(1, (int)WeatherType.NUMBEROFWEATHERTYPES);

            if (randomWeather != (int)_currWeather) {
                CheckIfWeatherTypeIsOn(randomWeather);
            } else {
                Debug.Log("We got the same weather no change will happen!");
            }

            _canChange = false;
        }

        void ExitCurrentWeather(int newWeather) {
            if (_currWeather == WeatherType.RANDOM)
            {
                // Set our last weather type
                _currWeather = WeatherType.RANDOM;

                // Enter new weather type
                EnterNewWeather(newWeather);

                // Reset values for changing weather
                _fTimeChangeWeatherStart = 0.0f;
                _bStartWeatherChange = false;
            }
            else if (_currWeather == WeatherType.SUN)
            {
                en_LastWeather = WeatherType.SUN;
                _fTimeChangeWeatherStart += Time.deltaTime;

                // Call the exit function in the weather type
                this.GetComponent<Weather_Sun>().ExitWeatherEffect(this.GetComponent<Weather_Sun>().GetSet_gSoundEffect);

                // After we reached or set time, we start to change into the new weather type
                if (_fTimeChangeWeatherStart >= _fTimeChangeWeatherEnd)
                {
                    this.GetComponent<Weather_Sun>().enabled = false;
                    EnterNewWeather(newWeather);

                    _fTimeChangeWeatherStart = 0.0f;
                    _bStartWeatherChange = false;
                }
            }
            else if (_currWeather == WeatherType.CLOUDY)
            {
                en_LastWeather = WeatherType.CLOUDY;
                _fTimeChangeWeatherStart += Time.deltaTime;

                // Call the exit function in the weather type
                this.GetComponent<Weather_Cloudy>().ExitWeatherEffect(this.GetComponent<Weather_Cloudy>().GetSet_gSoundEffect);

                // After we reached or set time, we start to change into the new weather type
                if (_fTimeChangeWeatherStart >= _fTimeChangeWeatherEnd)
                {
                    this.GetComponent<Weather_Cloudy>().enabled = false;
                    EnterNewWeather(newWeather);

                    _fTimeChangeWeatherStart = 0.0f;
                    _bStartWeatherChange = false;
                }
            }
            else if (_currWeather == WeatherType.RAIN)
            {
                en_LastWeather = WeatherType.RAIN;
                _fTimeChangeWeatherStart += Time.deltaTime;

                // Call the exit function in the weather type
                this.GetComponent<Weather_Rain>().ExitWeatherEffect(this.GetComponent<Weather_Rain>().GetSet_gPartRain);

                // After we reached or set time, we start to change into the new weather type

                if (_fTimeChangeWeatherStart >= _fTimeChangeWeatherEnd)
                {
                    this.GetComponent<Weather_Rain>().enabled = false;
                    EnterNewWeather(newWeather);

                    _fTimeChangeWeatherStart = 0.0f;
                    _bStartWeatherChange = false;
                }
            }
            else if (_currWeather == WeatherType.THUNDERSTORM)
            {
                en_LastWeather = WeatherType.THUNDERSTORM;
                _fTimeChangeWeatherStart += Time.deltaTime;

                // Call the exit function in the weather type
                this.GetComponent<Weather_Thunderstorm>().ExitWeatherEffect(this.GetComponent<Weather_Thunderstorm>().GetSet_gPartRain);

                // After we reached or set time, we start to change into the new weather type
                if (_fTimeChangeWeatherStart >= _fTimeChangeWeatherEnd)
                {
                    this.GetComponent<Weather_Thunderstorm>().enabled = false;
                    EnterNewWeather(newWeather);

                    _fTimeChangeWeatherStart = 0.0f;
                    _bStartWeatherChange = false;
                }
            }
            else if (_currWeather == WeatherType.SNOW)
            {
                en_LastWeather = WeatherType.SNOW;
                _fTimeChangeWeatherStart += Time.deltaTime;

                // Call the exit function in the weather type
                this.GetComponent<Weather_Snow>().ExitWeatherEffect(this.GetComponent<Weather_Snow>().GetSet_gPartSnow);

                // After we reached or set time, we start to change into the new weather type
                if (_fTimeChangeWeatherStart >= _fTimeChangeWeatherEnd)
                {
                    this.GetComponent<Weather_Snow>().enabled = false;
                    EnterNewWeather(newWeather);

                    _fTimeChangeWeatherStart = 0.0f;
                    _bStartWeatherChange = false;
                }
            }
        }

        void Update() {

        }
    }
}