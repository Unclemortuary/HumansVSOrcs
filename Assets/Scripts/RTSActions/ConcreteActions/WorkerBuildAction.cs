using UnityEngine;
using UnityEngine.AI;

public class WorkerBuildAction : AbstractRTSAction {

    private Identification.UnitType unitType;

//    private float timeToWait = 5f;

    private GameObject buildingGhost = null;
    private BuildingsConstructionHandler constructionHandler = null;

    private AbstractGameUnit performer = null;
    private UnitReactionsComponent performerReactions = null;
    private BuilderReactionsComponent performerBuilderReactions = null;


//    private State state;

//    private enum State {
//        SEARCHING_PLACE, WAITING_FOR_WORKER, BUILDING,
//    }


    public WorkerBuildAction(Identification.UnitType type) {
        this.unitType = type;

        Debug.Log("Worker BuildAction created");
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


                    newBuilding.Avatar.GetComponent<BuildingComponent>().SetTransparent(true);


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
        Debug.Log("Starting Worker's BuildAction");

        foreach(AbstractGameUnit unit in data.SelectedUnits) {
            performerBuilderReactions = unit.Avatar.GetComponent<BuilderReactionsComponent>();

            if (performerBuilderReactions != null) {
                performer = unit;
                performerReactions = unit.Avatar.GetComponent<UnitReactionsComponent>();
                Debug.Log("Found performer for building a Building");
                break;
            }
        }

        buildingGhost = data.ThisArmyManager.CreateBuildingGhost(unitType, Vector3.zero);
        constructionHandler = buildingGhost.AddComponent<BuildingsConstructionHandler>();

        NavMeshObstacle obstacle = buildingGhost.GetComponent<NavMeshObstacle>();
        if (obstacle != null) {
//            obstacle.carving = false;
            GameObject.Destroy(obstacle);
        }

        data.WaitingForTarget = true;

//        state = State.SEARCHING_PLACE;
    }


    public override void Stopping(ArmyStateData data) {

        Debug.Log("Quitting Worker's building action");

        if (buildingGhost != null) {
            GameObject.Destroy(buildingGhost);
        }

    }


}