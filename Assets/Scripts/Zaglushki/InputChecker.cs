using UnityEngine;
using UnityEngine.UI;

public class InputChecker : RTSMonoBehaviour {


    [SerializeField]
    private float mouseSpeed = 2;
    [SerializeField]
    private float mouseWheelSpeed = 3;



    [SerializeField]
    private CameraMoverV2 cameraMover;

    [SerializeField]
    private int currentUnitTypeIndex = 0;
    private int numOfTypes = 11;



    [SerializeField]
    private float borderDelta = 10;


    [SerializeField]
    private Sprite sun;
    [SerializeField]
    private Sprite moon;
    [SerializeField]
    private GameObject sunMoonButton;

    private Image sunMoonImage;
    private bool nowIsSun = true;

    public void SunMoonChange() {
        print("change");
        if (nowIsSun) {
            sunMoonImage.overrideSprite = moon;
        } else {
            sunMoonImage.overrideSprite = sun;
        }

        nowIsSun = !nowIsSun;
    }


// Use this for initialization
    void Start () {

        sunMoonImage = sunMoonButton.GetComponent<Image>();

        HUDscript.HideFloatingPanel();
    }


// Update is called once per frame
    void Update () {

//        print("mouse x = " + Input.mousePosition.x + ", screen width = " + Screen.width
//                            + ", screen height = " + Screen.height);

        if (Input.mousePosition.x >= Screen.width - borderDelta) {
            cameraMover.ShiftX(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.x <= borderDelta) {
            cameraMover.ShiftX(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }

        if (Input.mousePosition.y >= Screen.height-borderDelta) {
            cameraMover.ShiftZ(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.y <= borderDelta) {
            cameraMover.ShiftZ(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }


        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

//        cameraMover.ShiftX( Input.GetAxis( "Mouse X") * mouseSpeed );
//        cameraMover.ShiftZ( Input.GetAxis( "Mouse Y") * mouseSpeed );

        cameraMover.ShiftY( -Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed );



        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance == null) {
                print("instance = null");
            }
            if(GameManager.Instance.ArmyManagers[Identification.Army.Humans] == null) {
                print("HumanArmyManager == null");
            }
            CreateCurrentUnit(GameManager.Instance.ArmyManagers[Identification.Army.Humans]);
        }



        if (Input.GetMouseButtonDown(1))
        {
            CreateCurrentUnit(GameManager.Instance.ArmyManagers[Identification.Army.Orcs]);
        }


        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currentUnitTypeIndex = (currentUnitTypeIndex + 1) % numOfTypes;
        }

    } // Update //



    private void CreateCurrentUnit(ArmyManager armyManager) {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            GameUnitID unitId = hit.collider.gameObject.GetComponent<GameUnitID>();

            if (unitId != null) {

                ArmyManager clickedUnitArmyManager = GameManager.Instance.ArmyManagers[unitId.Army];

//                switch(unitId.Army) {
//                    case Identification.Army.Humans:
//                        clickedUnitArmyManager = GameManager.Instance.HumanArmyManager;
//                        break;
//                    case Identification.Army.Orcs:
//                        clickedUnitArmyManager = GameManager.Instance.OrcArmyManager;
//                        break;
//                }

                AbstractGameUnit unit = clickedUnitArmyManager.FindGameUnit(unitId.PersonalID);
                if (unit != null) {
                    HUDscript.ShowFloatingPanelAt(Input.mousePosition.x, Input.mousePosition.y, "[You clicked]:: " + unit.Description);
//                    HUDscript.Message("[You clicked]:: " + unit.Description);
                }

            } else {

//                print("Hitpoint=" + hit.point + ", currentTypeIndex=" + currentUnitTypeIndex + ":" + (Identification.UnitType)currentUnitTypeIndex);

                AbstractGameUnit unit = null;

                if (currentUnitTypeIndex < 3) {
                    unit = armyManager.CreateWarrior((Identification.UnitType)currentUnitTypeIndex, hit.point);
                } else {
                    unit = armyManager.CreateBuilding((Identification.UnitType)currentUnitTypeIndex, hit.point);
                }



                HUDscript.Message("Last created: " + unit.Description);
            }



        } // if raycast //

    } // Create current unit //


}