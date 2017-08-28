using UnityEngine;

public class BuildAction : AbstractRTSAction {

    private Identification.UnitType unitType;

//    private float timeToWait = 5f;

    private GameObject buildingGhost = null;
    private BuildingsConstructionHandler constructionHandler = null;

    public BuildAction(Identification.UnitType type) {
        this.unitType = type;

        Debug.Log("BuildAction created");
    }

    ////////////////////////////

    public override void Doing(ArmyStateData data) {
        if (data.WaitingForTarget) {

            Debug.Log("BuildingAction: Waiting for target");
            if (buildingGhost == null) {
                Debug.Log("Ghost is null");
            }
            if (constructionHandler == null) {
                Debug.Log("handler is null");
            }

        } else {

            if (constructionHandler.CanBuild) {

                if (data.TargetPointIsNowhere() && data.TargetUnit == null) {

                    Debug.Log("No Target position or unit. Can't create a unit");

                } else {
                    if (data.TargetPointIsNowhere()) {
                        data.TargetPoint = data.TargetUnit.Avatar.transform.position;
                    }

//                    GameObject.Destroy(constructionHandler);

                    AbstractGameUnit newBuilding = data.ThisArmyManager.CreateBuilding(unitType, data.TargetPoint);
                    GameObject scaffold = newBuilding.Avatar.transform.Find("BuildingBuild").gameObject;

                    if (scaffold != null) {
                        Debug.Log("scaffold is found");
                    }

                    Debug.Log("Setting scaffold active");
                    scaffold.SetActive(true);

                    new Timer(newBuilding.Avatar, delegate  {
                        scaffold.SetActive(false);
                    }, data.CurrentRtsAction.GetActionDataItem().TimeToComplete);

                }

                data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

            } else { // If Can Build //

                data.WaitingForTarget = true;

            }

        }

    } // Doing() //






    public override void Starting(ArmyStateData data) {
        Debug.Log("Starting BuildAction");

        buildingGhost = data.ThisArmyManager.CreateBuildingGhost(unitType, Vector3.zero);
        constructionHandler = buildingGhost.AddComponent<BuildingsConstructionHandler>();

        data.WaitingForTarget = true;
    }

    public override void Stopping(ArmyStateData data) {

        Debug.Log("Quitting building action");

        if (buildingGhost != null) {
            GameObject.Destroy(buildingGhost);
        }
//        data.ThisArmyManager.DestroyGameUnit(buildingGhost.ID);

    }



}