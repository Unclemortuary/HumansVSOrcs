using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShot : MonoBehaviour {

	public Transform arrowPrefab;
	public Transform arrowPosition;
	AnimatorStateInfo stateInfo;
	int arrowCreate = 0;

	int fireHasgState = Animator.StringToHash("Fire");

	void Update ()
	{
		ArrowShot ();
	}

	public void ShotState(int state)
	{
		arrowCreate = state;
	}

	void ArrowShot()
	{
		var anim = GetComponent<Animator> ();
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(stateInfo.tagHash == fireHasgState)
		{
			if (arrowCreate == 1) {
				Transform arrowInstance;
				arrowInstance = Instantiate (arrowPrefab, arrowPosition.position, arrowPosition.rotation) as Transform;
				arrowInstance.gameObject.GetComponent<Rigidbody> ().AddForce (arrowPosition.forward * 3000);
				arrowCreate = 0;
			}
		}
	}
}
