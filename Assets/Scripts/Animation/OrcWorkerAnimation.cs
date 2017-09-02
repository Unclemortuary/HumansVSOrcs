using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcWorkerAnimation : UnitAnimation  {

	// Update is called once per frame
	void Update () {
		AnimMove ();
		AnimWork ();
	}
		
	override public void AnimWork()
	{
		var stateMachine = GetComponent<UnitStateMachine> ();
		var anim = GetComponent<Animator> ();
		if (stateMachine.CurrentState == UnitStateMachine.State.BUILDING) {
			anim.SetBool ("BuildState", true);
		} else {
			anim.SetBool ("BuildState", false);
		}
	}
}
