using UnityEngine;
using System.Collections;


namespace Project.Weather
{
    public class Weather_Sun : Weather_Base
    {
        private void Start() {
            if (_isUseMorningFog == false) {
                _fogMorningAmount = _fogAmount;
            }
        }

        public override void Init() {
            base.Init();
        }

        private void Update() {
            UpdateWeather();
            if (_isUseInit) {
                _initTimerStart += Time.deltaTime;
                if (_initTimerStart >= _initTimerEnd){
                    Init();
                    _initTimerStart = 0.0f;
                    _isUseInit = false;
                }
            }
        }

        public override void ExitWeatherEffect() {
            base.ExitWeatherEffect();
        }
    }
}
