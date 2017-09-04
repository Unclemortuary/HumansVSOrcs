using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitsPathDraw : MonoBehaviour {

	// Update is called once per frame
	Vector3 prevPosition = Vector3.zero;
	void Update () {
		DrawPath();
	}

	void DrawPath()
	{
		var nav = GetComponent<NavMeshAgent> ();
		var proj  = gameObject.transform.Find("Projector").gameObject;
		var stateMachine = GetComponent<UnitStateMachine> ();
		if (nav == null || nav.path == null)
			return;

		var line = this.GetComponent<LineRenderer> ();

		if (line == null) {
			line = this.gameObject.AddComponent<LineRenderer> ();
			line.material = new Material (Shader.Find ("Sprites/Default")) { color = Color.yellow };
			line.SetWidth (0.35f, 0.35f);
			line.SetColors (Color.yellow, Color.yellow);
		}
		if (stateMachine.CurrentState == UnitStateMachine.State.MOVE_AND_ATTACK || stateMachine.CurrentState == UnitStateMachine.State.FOLLOW_AND_ATTACK) {
			line.SetColors (Color.red, Color.red);
		}

		line.enabled = true;
		if (proj.activeInHierarchy == false) {
			line.enabled = false;
		}
		if (Mathf.Abs (prevPosition.x - nav.nextPosition.x) < 0.01f && Mathf.Abs (prevPosition.z - nav.nextPosition.z) < 0.01f) {
			line.enabled = false;
		}

		var path = nav.path;
		line.SetVertexCount (path.corners.Length);

		for (int i = 0; i < path.corners.Length; i++) {
			line.SetPosition (i, path.corners [i]);
		}
		prevPosition = nav.nextPosition;
	}
}