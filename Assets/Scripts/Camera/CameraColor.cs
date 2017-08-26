using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor : MonoBehaviour {

	public float sphereRadius = 10.0f;
	public float raySize = 1000.0f;
	public Color myCam;
	public Color myRay;

	void OnDrawGizmos() 
	{
		Gizmos.color = myCam;
		Gizmos.DrawSphere(transform.position,  sphereRadius);
		Gizmos.color = myRay;
		Vector3 direction = transform.TransformDirection(Vector3.forward) * raySize;
		Gizmos.DrawRay(transform.position, direction);
	}

}
