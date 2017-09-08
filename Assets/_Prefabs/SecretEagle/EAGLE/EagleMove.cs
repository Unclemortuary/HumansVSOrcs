using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EagleMove : MonoBehaviour {

	public Camera mainCamera;
	public Camera eagleCamera;

	public GameObject partsMove;
	public GameObject partsWings1;
	public GameObject partsWings2;
	AnimatorStateInfo stateInfo;

	bool cameraState = true;
	public GameObject eagleParent;
	public GameObject eagleChild;
	public float speed = 3.0F;
	public float rotateSpeed = 3.0F;
	public CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	float num = 1.0f;
	float startSpeed;
	Animator eagleAnim;
	int endHasgState = Animator.StringToHash("Glide");
	public GameObject hud;
	public GameObject parts;
	public SkinnedMeshRenderer meshEagle;

	bool eagleView = false;
	Vector3 startPosition;

	void Start ()
	{
		startSpeed = speed;
		eagleAnim = eagleChild.GetComponent<Animator> ();
		startPosition =  new Vector3 (eagleParent.transform.position.x, eagleParent.transform.position.y,eagleParent.transform.position.z);
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Q)) {
			ChangeCameraState ();
		}
		EagleMover ();
		EagleView ();
	}

	void EagleView()
	{
		if(Input.GetKeyUp (KeyCode.V))
		{
			eagleView = !eagleView;
		}
		eagleCamera.gameObject.GetComponent<ColorCorrectionCurves> ().enabled = eagleView;
	}

	void ChangeCameraState()
	{
		cameraState = !cameraState;
		if (cameraState) {
			mainCamera.gameObject.SetActive (true);
			eagleCamera.gameObject.SetActive (false);
			hud.SetActive (true);
			parts.SetActive (false);
			meshEagle.enabled = false;
		} else {
			mainCamera.gameObject.SetActive (false);
			eagleCamera.gameObject.SetActive (true);
			hud.SetActive (false);
			parts.SetActive (true);
			meshEagle.enabled = true;
		}
	}

	void WingsPartsState()
	{
		stateInfo = eagleAnim.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.tagHash == endHasgState) {
			partsWings1.SetActive (true);
			partsWings2.SetActive (true);
		} else {
			partsWings1.SetActive (false);
			partsWings2.SetActive (false);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit col) 
	{
		if (col.collider.tag  != "Eagle") 
		{
			eagleParent.transform.position = new Vector3 (startPosition.x, startPosition.y, startPosition.z);
			ChangeCameraState ();
			//eagleParent.SetActive (false);
		}
	}


	void EagleMover()
	{
		if (!cameraState) {
			eagleParent.SetActive (true);	
			float moveForward = Input.GetAxis("Vertical");

			if (Input.GetAxis ("Vertical") < 0) {
				moveForward = 0.0f;
			}
				
			WingsPartsState ();

			float height = 0.0f;

			
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), height, moveForward);
			moveDirection = eagleParent.transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			controller.Move(moveDirection * Time.deltaTime);


			if(Input.GetKey (KeyCode.H))
			{
				//height = 0.5f;
				eagleParent.transform.Translate(Vector3.up * speed * Time.deltaTime);
			}

			if(Input.GetKey (KeyCode.J))
			{
				//height = -1.0f;
				eagleParent.transform.Translate(Vector3.down * speed * Time.deltaTime);
			}


			if ((Mathf.Abs (Input.GetAxis ("Vertical")) < 0.01f && Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.01f) 
				|| (Mathf.Abs (Input.GetAxis ("Vertical")) < 0.01f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.01f)) {
				eagleAnim.SetFloat ("Glide", 0.0f);
				partsMove.SetActive (false);
			} else {
				eagleAnim.SetFloat ("Glide", 1.0f);
				partsMove.SetActive (true);
			}

			if (Input.GetAxis ("Vertical") < 0) {
				eagleAnim.SetFloat ("Glide", -1f);
				partsMove.SetActive (false);
				partsWings1.SetActive (false);
				partsWings2.SetActive (false);
			}

			if (moveDirection != Vector3.zero) 
			{
				if (Mathf.Abs (Input.GetAxis ("Vertical")) < 0.01f && Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.01f) 
				{
					num = 0.5f;
					speed = startSpeed / 10;
				} else {
					num = 1.0f;
					speed = startSpeed;	
				}
				eagleParent.transform.rotation = Quaternion.Slerp (eagleParent.transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * rotateSpeed * num);
			}
		}
	}
}
