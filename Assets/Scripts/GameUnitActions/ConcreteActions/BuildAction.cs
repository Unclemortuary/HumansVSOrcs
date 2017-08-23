using UnityEngine;

public class BuildAction : AbstractRTSAction {

    private Identification.UnitType unitType;


    private AbstractGameUnit building = null;
    private BuildingsConstructionHandler constructionHandler = null;

    public BuildAction(Identification.UnitType type) {
        this.unitType = type;

        Debug.Log("BuildAction created");
    }

    ////////////////////////////

    public override void Doing(ArmyStateData data) {
        if (data.WaitingForTarget) {

            Debug.Log("BuildingAction: Waiting for target");

        } else {

            if (true /*constructionHandler.CanBuild*/) {

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

                    new Timer(building.Avatar, delegate  {
                        scaffold.SetActive(false);
                    }, 500f);

                }

                data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

            } else {

                data.WaitingForTarget = true;

            }

        }

    } // Doing() //






    public override void Starting(ArmyStateData data) {
        Debug.Log("Starting BuildAction");

        building = data.ThisArmyManager.CreateBuilding(unitType, Vector3.zero);
        constructionHandler = building.Avatar.AddComponent<BuildingsConstructionHandler>();

        data.WaitingForTarget = true;
    }

    public override void Stopping(ArmyStateData data) {

        data.ThisArmyManager.DestroyGameUnit(building.ID);

    }



}