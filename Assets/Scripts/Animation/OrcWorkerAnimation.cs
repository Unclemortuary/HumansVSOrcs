using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcWorkerAnimation : MonoBehaviour {

	// Use this for initialization
	Vector3 prevPosition = Vector3.zero;
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		AnimMove ();
	}

	void AnimMove()
	{
		var nav = GetComponent<NavMeshAgent> ();
		if (nav == null || nav.path == null)
			return;
		var anim = GetComponent<Animator> ();

		if (Mathf.Abs (prevPosition.x - nav.nextPosition.x) < 0.01f && Mathf.Abs (prevPosition.z - nav.nextPosition.z) < 0.01f) {
		//if(nav.destination != transform.position){
			anim.SetBool ("MoveState", false);
		} else {
			anim.SetBool ("MoveState", true);
		}



		prevPosition = nav.nextPosition;
	}
}
