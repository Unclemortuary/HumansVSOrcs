using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseRotate : MonoBehaviour {

	public float speed = 10.0f;
	public Vector3 direction;

	
	// Update is called once per frame
	void Update () {
		transform.Rotate (direction * speed * Time.deltaTime);
	}
}
