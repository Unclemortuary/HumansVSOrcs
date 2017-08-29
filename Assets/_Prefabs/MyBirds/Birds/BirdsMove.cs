using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsMove : MonoBehaviour {

	Vector3 StartPosition;
	public Transform[] positions;
	Vector3 EndPosition;
	Transform CurrentPosition;
	public float speed = 2;
	bool moveFlag = true;
	float minDst = 0.01f;     
	public int destPoint  = 0;
	//public int maxSize = 0;
	Transform myTransform;

	void Start() 
	{
		myTransform = gameObject.GetComponent<Transform>();
		speed = Random.Range (9, 14);
		//Pos2 = new Vector3 (5.0f, 5.0f, 0); 
		GoToPoint();
	}

	void Update () 
	{
			myTransform.position = Vector3.MoveTowards (transform.position, EndPosition, speed * Time.deltaTime);
			if (myTransform.position == EndPosition) 
			{
				GoToPoint ();
			}
	}

	void GoToPoint()
	{
		destPoint = (Random.Range(0, 12) + 1) % positions.Length;
		EndPosition = new Vector3(positions [destPoint].position.x, positions [destPoint].position.y,positions [destPoint].position.z );
		myTransform.LookAt (EndPosition);
		//destPoint = (destPoint + 1) % positions.Length;
	}
}

