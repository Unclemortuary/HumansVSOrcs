using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.TimeManager
{
    [System.Serializable]
    public class TimeManager : MonoBehaviour
    {
        [Range(0, 1)]
        private float _currentTime;

        private Transform _sun;
        private float _fullDay = 12f; // duration of the day, in seconds

        private IClock _clockView;
        private SunControler _sunController;

        private static TimeManager _instance;
        public static TimeManager GetInstance {
            get {
                return _instance;
            }
        }

        void Awake() {
            _instance = this;
        }

        public void Init(Transform sun) {
            _sun = sun;
            _sunController = new SunControler(_sun);
        }

        public void Init(Transform sun, IClock clockView) {
            Init(sun);
            SetClockView(clockView);
        }

        public void SetClockView(IClock clockView)
        {
            _clockView = clockView;
        }

        void Start() {
            _currentTime = 0.5f;
        }

        void Update() {
            _currentTime += Time.deltaTime / _fullDay;
            if (_currentTime >= 1) {
                _currentTime = 0;
            }
            else if (_currentTime < 0) {
                _currentTime = 0;
            }

            if (_clockView != null) {
                _clockView.ShowTime(_currentTime);
            }
            _sunController.MoveSun(_currentTime);
        }
    }
}
