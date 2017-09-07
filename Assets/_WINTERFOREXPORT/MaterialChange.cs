using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MaterialChange : MonoBehaviour {

	public GameObject winter;
	float progress = 0;
	public float speed = 10.0f;
	bool frozeState = true;
	public Material oneMat;
	public Material twoMat;
	public Material threeMat;
	public GameObject water;
	int sign = -1;

	bool startFroze = false;

	Material mat;
	void Start()
	{
		progress = 0;
		oneMat.SetFloat ("_Progress", progress);
		twoMat.SetFloat ("_Progress", progress);
		threeMat.SetFloat ("_Progress", progress);
	}


	void Update()
	{
		if (Input.GetKeyUp (KeyCode.O)) {
			startFroze = !startFroze;
		}
			ChangeMaterial ();
	}

	void ChangeMaterial()
	{

		if (startFroze) {
			sign = 1;
			water.GetComponent<NavMeshObstacle> ().enabled = false;
		} else {
			sign = -1;
			water.GetComponent<NavMeshObstacle> ().enabled = true;
		}


		progress += sign * speed * Time.deltaTime;
		if (progress <= 0)
			progress = 0;
		if (progress >= 1)
			progress = 1;
		oneMat.SetFloat ("_Progress", progress);
		twoMat.SetFloat ("_Progress", progress);
		threeMat.SetFloat ("_Progress", progress);
	}
}
