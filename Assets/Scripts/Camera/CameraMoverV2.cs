using UnityEngine;

public class CameraMoverV2 : MonoBehaviour {


    private Camera thisCamera;

    private GameObject cameraHolder;

	Vector3 startPosition;
	Vector3 endZoomPosition;

    [SerializeField]
    private float mouseSpeed = 3;
	[SerializeField]
	private float mouseSpeedMin = 0.8f;
    [SerializeField]
    private float mouseWheelSpeed = 4;

    /**
    Angle in degrees.
    Angle between Z-axis and camera view angle
     */
    [SerializeField]
    private float cameraViewAngle = 47;
    [SerializeField]
    private float cameraHeight = 200;

    [SerializeField]
    private float step = 0.3f;
    [SerializeField]
    private float epsilon = 0.0001f;


    [SerializeField]
    private Vector3 minPoint = new Vector3(0, 30, 0);
    [SerializeField]
    private Vector3 maxPoint = new Vector3(600,  30, 600);
    [SerializeField]
    private Vector3 moveAtPosition;


    [SerializeField]
    private float minCamSize = 5f;
    [SerializeField]
    private float maxCamSize = 50f;

    private float standardSize = 20f;

    [SerializeField]
    private float moveAtCamSize;

	public float MouseSpeed{
		get{return mouseSpeed;}
		set{mouseSpeed = value;}
	}


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
		moveAtPosition.x += shift * mouseSpeed; //* thisCamera.orthographicSize/standardSize;

        CorrectLimits();
    }

    public void ShiftY(float shift) {
        moveAtCamSize += shift * mouseWheelSpeed;

        CorrectLimits();
    }


    public void ShiftZ(float shift) {
		moveAtPosition.z += shift * mouseSpeed; // * thisCamera.orthographicSize/standardSize;

        CorrectLimits();
    }


    private void CorrectLimits() {
        //moveAtPosition.x = limitedValue(moveAtPosition.x,
               // minPoint.x + thisCamera.orthographicSize,
              //  maxPoint.x - thisCamera.orthographicSize);

        //moveAtPosition.z = limitedValue(moveAtPosition.z,
               // minPoint.z + thisCamera.orthographicSize,
              //  maxPoint.z - thisCamera.orthographicSize);

		moveAtPosition.x = limitedValue(moveAtPosition.x,minPoint.x ,maxPoint.x);
		moveAtPosition.z = limitedValue(moveAtPosition.z,minPoint.z ,maxPoint.z );

        moveAtPosition.y = limitedValue(moveAtPosition.y, minPoint.y, maxPoint.y);

        moveAtCamSize = limitedValue(moveAtCamSize, minCamSize, maxCamSize);
    }


    public void SetStartingPosition(Vector3 pos) {
        moveAtPosition = pos;
    }



    void Start () {
        thisCamera = gameObject.GetComponent<Camera>();
        cameraHolder = gameObject.transform.parent.gameObject;

        float newY = cameraHeight;
        float newZ = - newY / Mathf.Tan(cameraViewAngle * Mathf.PI / 180f);

        transform.localPosition = new Vector3(0, newY, newZ);

        Quaternion downRotation = Quaternion.AngleAxis(cameraViewAngle, Vector3.right);
        transform.rotation = downRotation;

        moveAtPosition = cameraHolder.transform.position;
        moveAtCamSize = thisCamera.orthographicSize;

		startPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		endZoomPosition = new Vector3 (0, -10.0f, 7.1f);

    } // Start() //


	void Zoom()
	{
		
		float movement = Input.GetAxis("Mouse ScrollWheel");
		if(movement == 0) 
		{
			return;
		}
		if (Mathf.Abs (thisCamera.transform.localPosition.y) <= maxCamSize && Mathf.Abs (thisCamera.transform.position.y) >= minCamSize)
		{
			Transform camTransform = Camera.main.transform;
			float distance = Vector3.Distance (camTransform.position, endZoomPosition) + 1;
			camTransform.Translate (camTransform.forward * distance * movement * Time.deltaTime * mouseWheelSpeed);
			thisCamera.transform.localPosition = new Vector3(thisCamera.transform.localPosition.x,
                    Mathf.Clamp (thisCamera.transform.localPosition.y, minCamSize, maxCamSize),
                    Mathf.Clamp (thisCamera.transform.localPosition.z, startPosition.z, startPosition.z));
		}
		mouseSpeed = Mathf.Abs(3.5f * thisCamera.transform.localPosition.y / startPosition.y);
		if (mouseSpeed < mouseSpeedMin) {
			mouseSpeed = mouseSpeedMin;
		}
	}




    void Update () 
	{
		
        if (cameraHolder.transform.position != moveAtPosition) {

            if ((cameraHolder.transform.position - moveAtPosition).sqrMagnitude < epsilon) {
                cameraHolder.transform.position = moveAtPosition;
            } else {

                cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, moveAtPosition, step);
            }

        }
		/*
        if (thisCamera.orthographicSize != moveAtCamSize) {
            if (thisCamera.orthographicSize - moveAtCamSize < epsilon) 
			{
                thisCamera.orthographicSize = moveAtCamSize;
            }
			else 
			{
                thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, moveAtCamSize, step);
            }
        }*/
		Zoom ();
    } // Update() //




} // End Of Class //
