using UnityEngine;
using System.Collections;

namespace Project.Weather
{
    public class Weather_Rain : Weather_Base
    {
        [SerializeField]
        private GameObject _rainParticle;

        private float _endParticleTimerStart;
        private float _endParticleTimerEnd;

        public GameObject RainParticle {
            get { return _rainParticle; }
            set { _rainParticle = value; }
        }

        private void Start() {
            if (!_isUseMorningFog) {
                _fogMorningAmount = _fogAmount;
            }

            // This timer makes sure that the rain stops falling and don't suddenly just disappears
            _endParticleTimerStart = 0.0f;
            _endParticleTimerEnd = 3.0f;
        }

        public override void Init() {
            base.Init();
            TurnOnRain();

            // We turn on emission on the particle system as we always turn it off in the end
            if (_rainParticle != null) {
                ParticleSystem.EmissionModule em = _rainParticle.GetComponent<ParticleSystem>().emission;
                em.enabled = true;
            }
        }

        private void Update() {
            UpdateWeather();
            if (_isUseInit) {
                _initTimerStart += Time.deltaTime;
                if (_initTimerStart >= _initTimerEnd) {
                    Init();
                    _initTimerStart = 0.0f;
                    _isUseInit = false;
                }
            }
        }

        private void TurnOnRain() {
            if (_rainParticle != null) {
                if (!_rainParticle.activeInHierarchy) {
                    _rainParticle.SetActive(true);
                }
            } else {
                Debug.Log("We are missing rain particles on: " + this.gameObject + " For weather type: RAIN");
            }
        }

        public override void ExitWeatherEffect() {
            base.ExitWeatherEffect();
            if (_rainParticle != null && _rainParticle.activeInHierarchy) {
                _endParticleTimerStart += Time.deltaTime;
                ParticleSystem.EmissionModule em = _rainParticle.GetComponent<ParticleSystem>().emission;
                em.enabled = false;

                if (_endParticleTimerStart > _endParticleTimerEnd) {
                    _endParticleTimerStart = 0.0f;
                    _rainParticle.SetActive(false);
                }
            }
        }
    }
}