using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour{

	Vector3 prevPosition = Vector3.zero;
	virtual public void AnimMove()
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

	virtual public void  AnimArrack()
	{
	}

	virtual public void AnimBuild()
	{
	}
}
