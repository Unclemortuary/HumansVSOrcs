using UnityEngine;
using System.Collections;


namespace Project.Weather
{
    public class Weather_Snow : Weather_Base
    {
        [SerializeField]
        private GameObject _snowParticle;

        private float _endParticleTimerStart;
        private float _endParticleTimerEnd;

        public GameObject SnowParticle {
            get { return _snowParticle; }
            set { _snowParticle = value; }
        }

        private void Start() {
            if (_isUseMorningFog == false) {
                _fogMorningAmount = _fogAmount;
            }

            // This timer makes sure that the rain stops falling and don't suddenly just disappears
            _endParticleTimerStart = 0.0f;
            _endParticleTimerEnd = 3.0f;
        }

        public override void Init() {
            base.Init();
            TurnOnSnow();

            // We turn on emission on the particle system as we always turn it off in the end
            if (_snowParticle != null) {
                ParticleSystem.EmissionModule em = _snowParticle.GetComponent<ParticleSystem>().emission;
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

        private void TurnOnSnow() {
            if (_snowParticle != null) {
                if (!_snowParticle.activeInHierarchy) {
                    _snowParticle.SetActive(true);
                }
            } else {
                Debug.Log("We are missing snow particles on:" + this.gameObject);
            }
        }

        public override void ExitWeatherEffect() {
            base.ExitWeatherEffect();
            if (_snowParticle != null && _snowParticle.activeInHierarchy) {
                _endParticleTimerStart += Time.deltaTime;
                ParticleSystem.EmissionModule em = _snowParticle.GetComponent<ParticleSystem>().emission;
                em.enabled = false;
                if (_endParticleTimerStart > _endParticleTimerEnd) {
                    _endParticleTimerStart = 0.0f;
                    _snowParticle.SetActive(false);
                }
            }
        }
    }
}