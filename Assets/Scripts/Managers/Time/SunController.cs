using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.TimeManager
{
    public class SunControler
    {
        private Transform _sun;

        internal SunControler(Transform sun) {
            _sun = sun;
        }

        public void MoveSun(float currentTime) {
            //_sun.localRotation = Quaternion.Euler((currentTime * 360f) - 90, 170, 0);
			//динамические тени очень очень плохо
        }
    }
}
