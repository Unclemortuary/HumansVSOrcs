using UnityEngine;

public class InputReader : MonoBehaviour{


    [SerializeField]
    private float mouseSpeed = 3;
    [SerializeField]
    private float mouseWheelSpeed = 4;

    [SerializeField]
    private float borderDelta = 10;


    [SerializeField]
    private CameraMoverV2 cameraMover;
    private Camera gameCamera;

    // Units selection using mouse //
    private bool isSelecting = false;
    private Vector3 mousePosition1;
    private float minSelectionDiam = 30;
    private Color screenRectColor = new Color( 0.8f, 0.8f, 0.95f, 0.25f );
    private Color screenRectBorder = new Color( 0.8f, 0.8f, 0.95f );
    // + + + + + + + + + + + + + + //

    void Start() {
        gameCamera = cameraMover.gameObject.GetComponent<Camera>();
    }

    void Update() {

        CheckExitApplication();

        MoveCamera();

        MouseSelectionCheck();


    }


    //////////////////////////////////////////////////////////////////////////////////////////


    private void CheckExitApplication() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }



    private void MoveCamera() {
        if (Input.mousePosition.x >= Screen.width - borderDelta) {
            cameraMover.ShiftX(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.x <= borderDelta) {
            cameraMover.ShiftX(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }

        if (Input.mousePosition.y >= Screen.height - borderDelta) {
            cameraMover.ShiftZ(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.y <= borderDelta) {
            cameraMover.ShiftZ(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }


        cameraMover.ShiftY(-Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed);
    }


    void MouseSelectionCheck()
    {
        if( Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp (0)) {
            isSelecting = false;

            // Check if units were selected //
            Vector3 mouseposition2 = Input.mousePosition;
            if (Vector3.Distance(mousePosition1, mouseposition2) > minSelectionDiam) {
                Debug.Log("selectiondiam = " + Vector3.Distance(mousePosition1, mouseposition2));
                Bounds viewportBounds = MouseRectTools.GetViewportBounds(gameCamera, mousePosition1, mouseposition2);
                GameManager.Instance.PlayerController.NewUnitsSelection(viewportBounds);
            }

        }

    }



    void OnGUI()
    {
        if(isSelecting)
        {
            // Создаем прямоугольник на основе начальных и конечных координат курсора
            Rect rect = MouseRectTools.GetScreenRect( mousePosition1, Input.mousePosition );
            MouseRectTools.DrawScreenRect( rect,  screenRectColor);
            MouseRectTools.DrawScreenRectBorder( rect, 2,  screenRectBorder);
        }
    }





}