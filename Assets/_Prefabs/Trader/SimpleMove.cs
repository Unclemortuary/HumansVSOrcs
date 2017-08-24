using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour {

	public Vector3 direction;
	public float speed = 10.0f;
	// Update is called once per frame
	void Update () {
		transform.Translate (direction * speed * Time.deltaTime);
	}
}
