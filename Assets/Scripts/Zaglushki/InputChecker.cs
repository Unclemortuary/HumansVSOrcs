using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {




    [SerializeField]
    private int currentUnitTypeIndex = 0;
    private int numOfTypes = 11;




//    [SerializeField]
//    private Sprite sun;
//    [SerializeField]
//    private Sprite moon;
//    [SerializeField]
//    private GameObject sunMoonButton;
//
//    private Image sunMoonImage;
//    private bool nowIsSun = true;
//
//    public void SunMoonChange() {
//        print("change");
//        if (nowIsSun) {
//            sunMoonImage.overrideSprite = moon;
//        } else {
//            sunMoonImage.overrideSprite = sun;
//        }
//
//        nowIsSun = !nowIsSun;
//    }
//

// Use this for initialization
    void Start () {

//        sunMoonImage = sunMoonButton.GetComponent<Image>();

//        HUDscript.HideFloatingPanel();
    }


// Update is called once per frame
    void Update () {

//        print("mouse x = " + Input.mousePosition.x + ", screen width = " + Screen.width
//                            + ", screen height = " + Screen.height);



/*
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            print("0 is clicked - StopMachine!");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
                    .Dispatcher.TriggerCommand(ArmyMessageTypes.stopMachine);
            GameManager.Instance.ArmyManagers[Identification.Army.Orcs]
                    .Dispatcher.TriggerCommand(ArmyMessageTypes.stopMachine);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            print("1 is clicked - Is Dsipatcher activated?");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
                    .Dispatcher.TriggerCommand(ArmyMessageTypes.addUnitsToSelection);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            print("2 is clicked - Trying to go to DoAction state");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
                    .Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                                RTSActionType.moveTo);
        }
*/
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            print("0 is clicked - StopMachine!");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
            .Dispatcher.TriggerCommand(ArmyMessageTypes.stopMachine);
            GameManager.Instance.ArmyManagers[Identification.Army.Orcs]
            .Dispatcher.TriggerCommand(ArmyMessageTypes.stopMachine);
        }


        if (Input.GetKeyDown(KeyCode.M)) {
            print("M is clicked - Trying to go to DoAction state");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
            .Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                    RTSActionType.moveTo);
        }


        if (Input.GetKeyDown(KeyCode.S)) {
            print("M is clicked - It means Stop selected units");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
            .Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                    RTSActionType.stop);
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            print("U is clicked - It means Create a unit");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
            .Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                    RTSActionType.createArcher);
        }


        if (Input.GetKeyDown(KeyCode.B)) {
            print("B is clicked - It means Build a construction");
            GameManager.Instance.ArmyManagers[Identification.Army.Humans]
            .Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                    RTSActionType.buildFarm);
        }



//        cameraMover.ShiftX( Input.GetAxis( "Mouse X") * mouseSpeed );
//        cameraMover.ShiftZ( Input.GetAxis( "Mouse Y") * mouseSpeed );



//        if (Input.GetMouseButtonDown(0))
//        {
//            if (GameManager.Instance == null) {
//                print("instance = null");
//            }
//            if(GameManager.Instance.ArmyManagers[Identification.Army.Humans] == null) {
//                print("HumanArmyManager == null");
//            }
////            CreateCurrentUnit(GameManager.Instance.ArmyManagers[Identification.Army.Humans]);
//        }
//


        if (Input.GetMouseButtonDown(1))
        {
            CreateCurrentUnit(GameManager.Instance.ArmyManagers[Identification.Army.Humans]);
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