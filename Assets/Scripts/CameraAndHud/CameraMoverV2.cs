using UnityEngine;

public class CameraMoverV2 : MonoBehaviour {


    private Camera thisCamera;

    private GameObject cameraHolder;


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
    private float maxCamSize = 20f;

    [SerializeField]
    private float moveAtCamSize;




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
        moveAtPosition.x += shift;

        CorrectLimits();
    }

    public void ShiftY(float shift) {
        moveAtCamSize += shift;

        CorrectLimits();
    }


    public void ShiftZ(float shift) {
        moveAtPosition.z += shift;

        CorrectLimits();
    }


    private void CorrectLimits() {
        moveAtPosition.x = limitedValue(moveAtPosition.x,
                minPoint.x + thisCamera.orthographicSize,
                maxPoint.x - thisCamera.orthographicSize);

        moveAtPosition.z = limitedValue(moveAtPosition.z,
                minPoint.z + thisCamera.orthographicSize,
                maxPoint.z - thisCamera.orthographicSize);

        moveAtPosition.y = limitedValue(moveAtPosition.y, minPoint.y, maxPoint.y);

        moveAtCamSize = limitedValue(moveAtCamSize, minCamSize, maxCamSize);
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
    } // Start() //






    void Update () {

        if (cameraHolder.transform.position != moveAtPosition) {

            if ((cameraHolder.transform.position - moveAtPosition).sqrMagnitude < epsilon) {
                cameraHolder.transform.position = moveAtPosition;
            } else {

                cameraHolder.transform.position =
                        Vector3.Lerp(cameraHolder.transform.position, moveAtPosition, step);
            }

        }

        if (thisCamera.orthographicSize != moveAtCamSize) {
            if (thisCamera.orthographicSize - moveAtCamSize < epsilon) {
                thisCamera.orthographicSize = moveAtCamSize;
            } else {

                thisCamera.orthographicSize =
                        Mathf.Lerp(thisCamera.orthographicSize, moveAtCamSize, step);
            }
        }

    } // Update() //




} // End Of Class //
