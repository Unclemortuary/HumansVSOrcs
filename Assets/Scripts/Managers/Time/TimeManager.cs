﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.TimeManager
{
    [System.Serializable]
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager _instance;
        public static TimeManager GetInstance {
            get {
                return _instance;
            }
        }


        [Range(0, 1)]
		public float _currTime;
        public float CurrTime {
            get { return _currTime; }
        }
        private TimeOfDay _currTimeOfDay;
        public TimeOfDay CurrTimeOfDay{
            get { return _currTimeOfDay; }
        }

        public TimeOfDay _currDebugTimeOfDay;

        private const float ONEHOURLENGTH = 1.0f / 24.0f;


        private int _iStartSunrise = 10;
        private int _iStartDay = 12;
        private int _iStartSunset = 18;
        private int _iStartNight = 20;

        public float _fullDay = 900f; // duration of the day, in seconds


        private float _fStartSunrise;
        private float _fStartDay;
        private float _fStartSunset;
        private float _fStartNight;

        private Transform _sun;
        private IClock _clockView;
        private SunControler _sunController;

        public Light sunLight;

        void Awake() {
            _instance = this;
        }

        public void Init(Transform sun) {
            _sun = sun;
            _sunController = new SunControler(_sun);
            sunLight = _sun.GetComponent<Light>();
        }

        public void Init(Transform sun, IClock clockView) {
            Init(sun);
            SetClockView(clockView);
        }

        public void SetClockView(IClock clockView) {
            _clockView = clockView;
        }

        void Start() {
            _currTime = 0.5f;
            _currTimeOfDay = TimeOfDay.DAY;
            _currDebugTimeOfDay = TimeOfDay.DAY;
            _fStartSunrise = ConvertTimeToFloat(_iStartSunrise);
            _fStartDay = ConvertTimeToFloat(_iStartDay);
            _fStartSunset = ConvertTimeToFloat(_iStartSunset);
            _fStartNight = ConvertTimeToFloat(_iStartNight);
        }

        private float ConvertTimeToFloat(int hour) {
            return ONEHOURLENGTH * (float)hour;
        }

        void Update() {
            if (_currDebugTimeOfDay != _currTimeOfDay)
            {
                switch (_currDebugTimeOfDay) {
                    case TimeOfDay.SUNRISE:
                        _currTime = _fStartSunrise;
                        break;
                    case TimeOfDay.DAY:
                        _currTime = _fStartDay;
                        break;
                    case TimeOfDay.SUNSET:
                        _currTime = _fStartSunset;
                        break;
                    case TimeOfDay.NIGHT:
                        _currTime = _fStartNight;
                        break;
                }

            }

            _currTime += Time.deltaTime / _fullDay;
            if (_currTime >= 1) {
                _currTime = 0;
            }
            else if (_currTime < 0) {
                _currTime = 0;
            }

            UpdateTimeOfDay();
            _sunController.MoveSun(_currTime);

            if (_clockView != null) {
                _clockView.ShowTime(_currTime);
            }
        }

        private void UpdateTimeOfDay() {
            if (_currTime >= _fStartSunrise && _currTime <= _fStartDay && _currTimeOfDay != TimeOfDay.SUNRISE) {
                SetCurrentTimeOfDay(TimeOfDay.SUNRISE);
            } else if (_currTime >= _fStartDay && _currTime <= _fStartSunset && _currTimeOfDay != TimeOfDay.DAY) {
                SetCurrentTimeOfDay(TimeOfDay.DAY);
            } else if (_currTime >= _fStartSunset && _currTime <= _fStartNight && _currTimeOfDay != TimeOfDay.SUNSET) {
                SetCurrentTimeOfDay(TimeOfDay.SUNSET);
            } else if (_currTime >= _fStartNight || _currTime <= _fStartSunrise && _currTimeOfDay != TimeOfDay.NIGHT) {
                SetCurrentTimeOfDay(TimeOfDay.NIGHT);
            }
        }

        private void SetCurrentTimeOfDay(TimeOfDay currTimeOfDay) {
            _currTimeOfDay = currTimeOfDay;
            _currDebugTimeOfDay = currTimeOfDay;
        }
    }
}
