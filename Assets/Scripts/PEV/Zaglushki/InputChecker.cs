using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {


    private float mouseSpeed = 2;
    private float mouseWheelSpeed = 3;



    [SerializeField]
    private CameraMover cameraMover;

    [SerializeField]
    private int currentUnitTypeIndex = 0;




    [SerializeField]
    private Sprite sun;
    [SerializeField]
    private Sprite moon;
    [SerializeField]
    private GameObject sunMoonButton;

    private Image sunMoonImage;
    private bool nowIsSun = true;

    public void SunMoonChange() {
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

        if (Input.mousePosition.x >= Screen.width) {
            cameraMover.ShiftX(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.x <= 0) {
            cameraMover.ShiftX(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }

        if (Input.mousePosition.y >= Screen.height) {
            cameraMover.ShiftZ(mouseSpeed);
            HUDscript.HideFloatingPanel();
        } else if (Input.mousePosition.y <= 0) {
            cameraMover.ShiftZ(-mouseSpeed);
            HUDscript.HideFloatingPanel();
        }



//        cameraMover.ShiftX( Input.GetAxis( "Mouse X") * mouseSpeed );
//        cameraMover.ShiftZ( Input.GetAxis( "Mouse Y") * mouseSpeed );

        cameraMover.ShiftY( -Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed );



        if (Input.GetMouseButtonDown(0))
        {
            CreateCurrentUnit(GameManager.Instance.HumanArmyManager);
        }



        if (Input.GetMouseButtonDown(1))
        {
            CreateCurrentUnit(GameManager.Instance.OrcArmyManager);
        }


        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currentUnitTypeIndex = (currentUnitTypeIndex + 1) % 5;
        }

    } // Update //



    private void CreateCurrentUnit(ArmyManager armyManager) {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            GameUnitID unitId = hit.collider.gameObject.GetComponent<GameUnitID>();

            if (unitId != null) {

                ArmyManager clickedUnitArmyManager = null;

                switch(unitId.Army) {
                    case Identification.Army.Humans:
                        clickedUnitArmyManager = GameManager.Instance.HumanArmyManager;
                    break;
                    case Identification.Army.Orcs:
                        clickedUnitArmyManager = GameManager.Instance.OrcArmyManager;
                    break;
                }

                AbstractGameUnit unit = clickedUnitArmyManager.FindGameUnit(unitId.PersonalID);
                if (unit != null) {
                    HUDscript.ShowFloatingPanelAt(Input.mousePosition.x, Input.mousePosition.y, "[You clicked]:: " + unit.Description);
//                    HUDscript.Message("[You clicked]:: " + unit.Description);
                }

            } else {

                print("Hitpoint=" + hit.point);

                AbstractGameUnit unit = null;

                switch (currentUnitTypeIndex) {
                    case 0:
                        unit = armyManager.CreateWarrior(Identification.UnitType.Archer, hit.point);
                        break;
                    case 1:
                        unit = armyManager.CreateWarrior(Identification.UnitType.Swordsman, hit.point);
                        break;
                    case 2:
                        unit = armyManager.CreateWarrior(Identification.UnitType.Horseman, hit.point);
                        break;
                    case 3:
                        unit = armyManager.CreateBuilding(Identification.UnitType.Palace, hit.point);
                        break;
                    case 4:
                        unit = armyManager.CreateBuilding(Identification.UnitType.Barrack, hit.point);
                        break;
                }


                HUDscript.Message("Last created: " + unit.Description);
            }



        } // if raycast //

    } // Create current unit //

}
