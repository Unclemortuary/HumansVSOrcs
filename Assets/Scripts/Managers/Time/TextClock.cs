using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.TimeManager { 
    public class TextClock : IClock  {

        private GameObject _textClock;
        private Text _textTime; // current time in "00:00" format

        private float h, m;
        private string hour, min;

        internal TextClock(Canvas _canvas) {
            _textClock = new GameObject("Text");
            _textClock.transform.SetParent(_canvas.transform);

            RectTransform _rectTransform = _textClock.AddComponent<RectTransform>();
            _rectTransform.anchoredPosition = new Vector2(160, 30);

            _textTime = _textClock.AddComponent<Text>();
            _textTime.text = "00:00";
            _textTime.fontSize = 20;
        } 

        public void ShowTime(float currentTime) {
            h = 24 * currentTime;
            m = 60 * (h - Mathf.Floor(h));

            if (m < 10) {
                min = "0" + (int)m;
            } else {
                min = ((int)m).ToString();
            }

            if (h < 10) {
                hour = "0" + (int)h;
            } else {
                hour = ((int)h).ToString();
            }

            _textTime.text = hour + ":" + min;
        }

    }
}
