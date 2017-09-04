using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcSwordManAnimation : UnitAnimation  {

	void Update () {
		AnimMove ();
		//AnimAttack ();
	}

	override public void AnimAttack()
	{
		var stateMachine = GetComponent<UnitStateMachine> ();
		var anim = GetComponent<Animator> ();

		//anim.SetBool ("AttackState", true);

		//anim.SetBool ("AttackState", false);

	}
}
