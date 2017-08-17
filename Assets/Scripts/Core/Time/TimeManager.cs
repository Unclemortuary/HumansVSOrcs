using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.TimeManager
{
    public class TimeManager : MonoBehaviour
    {
        [Range(0, 1)]
        private float _currentTime;

        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private Transform _sun;

        private float _fullDay = 120f; // duration of the day, in seconds

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
            _clockView = new TextClock(_canvas);
            _sunController = new SunControler(_sun);
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

            _clockView.ShowTime(_currentTime);
            _sunController.MoveSun(_currentTime);
        }
    }
}
