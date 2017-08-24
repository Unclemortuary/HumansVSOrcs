using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour {

	public float sphereRadius = 10.0f;
	public float raySize = 1000.0f;

	void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position,  sphereRadius);
		Gizmos.color = Color.red;
		Vector3 direction = transform.TransformDirection(Vector3.forward) * raySize;
		Gizmos.DrawRay(transform.position, direction);
	}

}
