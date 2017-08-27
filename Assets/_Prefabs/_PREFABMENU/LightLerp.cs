using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour {

	float startInt = 3.0f;
	float endInt = 8.0f;
	float t = 0.0f;
	public Light light;
	// Update is called once per frame
	void Update () {
		//light.intensity = Mathf.Lerp (startInt, endInt, t);
		light.intensity = 2.0f + 6 * Mathf.PingPong(Time.time , 1);
		t += 0.5f * Time.deltaTime;
	}
}
