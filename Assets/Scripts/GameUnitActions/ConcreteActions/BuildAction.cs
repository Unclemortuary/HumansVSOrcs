using UnityEngine;

public class BuildAction : AbstractRTSAction {

    private Identification.UnitType unitType;


    private AbstractGameUnit building = null;
    private BuildingsConstructionHandler constructionHandler = null;

    public BuildAction(Identification.UnitType type) {
        this.unitType = type;
    }

    ////////////////////////////

    public override void Doing(ArmyStateData data) {
        if (data.WaitingForTarget) {

            Debug.Log("BuildingAction: Waiting for target");

        } else {

            if (constructionHandler.CanBuild) {

                if (data.TargetPointIsNowhere() && data.TargetUnit == null) {

                    Debug.Log("No Target position or unit. Can't create a unit");

                } else {
                    if (data.TargetPointIsNowhere()) {
                        data.TargetPoint = data.TargetUnit.Avatar.transform.position;
                    }

                    GameObject.Destroy(constructionHandler);
                    GameObject scaffold = building.Avatar.transform.Find("BuildingBuild").gameObject;

                    scaffold.SetActive(true);

                    new Timer(building.Avatar, delegate  {
                        scaffold.SetActive(false);
                    }, 5f);

                }

                data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

            } else {

                data.WaitingForTarget = true;

            }

        }

    } // Doing() //






    public override void Starting(ArmyStateData data) {
        building = data.ThisArmyManager.CreateBuilding(unitType, Vector3.zero);
        constructionHandler = building.Avatar.AddComponent<BuildingsConstructionHandler>();

        data.WaitingForTarget = true;
    }

    public override void Stopping(ArmyStateData data) {

    }



}