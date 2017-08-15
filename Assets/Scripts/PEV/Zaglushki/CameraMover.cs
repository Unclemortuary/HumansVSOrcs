using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

//    private Rect mapRect;
//    public Rect MapRect {
//        get {
//            return mapRect;
//        }
//        set {
//            mapRect = value;
//        }
//    }
//
//    private float minDistance = 10;
//    private float maxDistan ;

    private Camera thisCamera;

    private float CameraHeight = 50f;

    [SerializeField]
    private Vector3 minPoint = new Vector3(7, 35, 5);
    [SerializeField]
    private Vector3 maxPoint = new Vector3(594,  35, 594);

    private float minCamSize = 5f;
    private float maxCamSize = 20f;
    private float moveAtCamSize;


    [SerializeField]
    private Vector3 moveAtPosition;



    private float step = 0.3f;
    private float epsilon = 0.001f;

    private float limitedValue(float value, float min, float max) {
        if (value < min) {
            return min;
        }
        if (value > max) {
            return max;
        }
        return value;
    }

    public void ShiftX(float shift) {

//        float cameraWidght = thisCamera.orthographicSize * Screen.width / Screen.height;
//        print("cwidth = " + cameraWidght + "  osize=" + thisCamera.orthographicSize);

        moveAtPosition.x = limitedValue(moveAtPosition.x + shift,
                minPoint.x,
                maxPoint.x);
    }

    public void ShiftY(float shift) {
//        print("shifty=" + shift);

        moveAtCamSize = limitedValue(moveAtCamSize + shift, minCamSize, maxCamSize);
    }


    public void ShiftZ(float shift) {
        moveAtPosition.z = limitedValue(moveAtPosition.z + shift,
                minPoint.z - 28,
                maxPoint.z - 27);
    }




	// Use this for initialization
	void Start () {
        thisCamera = gameObject.GetComponent<Camera>();

		moveAtPosition = transform.position;
        minPoint.y = moveAtPosition.y;
        maxPoint.y = moveAtPosition.y;

        moveAtCamSize = thisCamera.orthographicSize;

//        moveAtPosition = new Vector3(100, 100, 100);
	} // Start() //

	
	// Update is called once per frame
	void Update () {

        if (transform.position != moveAtPosition) {

            if ((transform.position - moveAtPosition).sqrMagnitude < epsilon) {
                transform.position = moveAtPosition;
            } else {

                transform.position = Vector3.Lerp(transform.position, moveAtPosition, step);
            }

        }

        if (thisCamera.orthographicSize != moveAtCamSize) {
            if (thisCamera.orthographicSize - moveAtCamSize < epsilon) {
                thisCamera.orthographicSize = moveAtCamSize;
            } else {

                thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, moveAtCamSize, step);
            }
        }

	} // Update() //



} // End Of Class //

