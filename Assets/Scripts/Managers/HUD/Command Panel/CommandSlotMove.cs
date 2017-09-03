using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSlotMove : MonoBehaviour {


	RectTransform slotTrans;
	void Awake () {
		slotTrans = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		slotTrans.position = new Vector3(Input.mousePosition.x - slotTrans.rect.width/2,Input.mousePosition.y + slotTrans.rect.height/2,slotTrans.position.z );
	}
}
