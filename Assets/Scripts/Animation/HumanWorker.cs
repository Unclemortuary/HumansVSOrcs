using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWorker : UnitAnimation {

	void Update () {
		AnimMove ();
		AnimWork ();
	}

	override public void AnimWork()
	{
		var stateMachine = GetComponent<UnitStateMachine> ();
		var anim = GetComponent<Animator> ();
		if (stateMachine == null)
			return;

		if (stateMachine.CurrentState == UnitStateMachine.State.BUILDING) {
			anim.SetBool ("BuildState", true);
		} else {
			anim.SetBool ("BuildState", false);
		}
	}
}