using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour {
	
	// Update is called once per frame

	bool moveState = true;
	//void FixedUpdate () {
		//Move ();
	//}

	void OnTriggerExit(Collider col)
	{
		if (col.tag != "Archer" && col.tag != "Warrior") {
			moveState = false;
			gameObject.GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	void Move()
	{
		if (moveState) {
			transform.Translate (Vector3.forward * 50.0f * Time.deltaTime);
		}
	}
}
