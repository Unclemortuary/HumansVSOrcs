using UnityEngine;
using UnityEngine.AI;

public class WorkerBuildAction : AbstractRTSAction {

    private Identification.UnitType unitType;

//    private float timeToWait = 5f;

    private GameObject buildingGhost = null;
    private BuildingsConstructionHandler constructionHandler = null;

    private AbstractGameUnit performer = null;
//    private UnitReactionsComponent performerReactions = null;
//    private BuilderReactionsComponent performerBuilderReactions = null;


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

//            Debug.Log("WorkerBuildingAction: Waiting for target");
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

                    if(data == null) {
                        Debug.Log("data is null");
                    } else if (data.ThisArmyManager == null) {
                        Debug.Log("thisArmyManager is null");
                    } else if (data.ThisArmyManager.Dispatcher == null) {
                        Debug.Log("Dispatcher is null");
                    } else if (newBuilding == null) {
                        Debug.Log("newBuilding is null");
                    } else if (performer == null) {
                        Debug.Log("performer is null");
                    }

                    data.ThisArmyManager.Dispatcher.TriggerCommand<float>(
                            ArmyMessageTypes.unitCommandSetWorkDuration,
                            data.CurrentRtsAction.GetActionDataItem().TimeToComplete,
                            performer.ID);

                    data.ThisArmyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(
                            ArmyMessageTypes.unitCommandBuild, newBuilding, performer.ID);


                    BuildingComponent buildingComponent = newBuilding.Avatar.GetComponent<BuildingComponent>();
                    buildingComponent.ChangeGridMapStatus(newBuilding.Avatar.transform.position);


                    // Pay the price //
                    data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);


                }

                data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

            } else { // If Can Build //

                Debug.Log("Can't build here!!!!!");

                data.WaitingForTarget = true;

            }

        }

    } // Doing() //






    public override void Starting(ArmyStateData data) {
        Debug.Log("Starting Worker's BuildAction");

        foreach(AbstractGameUnit unit in data.SelectedUnits) {

            if(unit.IsActive) {
//                performerBuilderReactions = unit.Avatar.GetComponent<BuilderReactionsComponent>();

//                if (performerBuilderReactions != null) {
                    performer = unit;
//                    performerReactions = unit.Avatar.GetComponent<UnitReactionsComponent>();
                    Debug.Log("Found performer for building a Building");
                    break;
//                }
            }
        }

        buildingGhost = data.ThisArmyManager.CreateBuildingGhost(unitType, Vector3.zero);



//        constructionHandler = buildingGhost.AddComponent<BuildingsConstructionHandler>();

        ResourceBuilding rb = buildingGhost.GetComponent<ResourceBuilding>();
        if (rb != null) {
			constructionHandler = buildingGhost.AddComponent<ResourcePlaceChecker>();
        } else {
            constructionHandler = buildingGhost.AddComponent<BuildingsConstructionHandler>();
        }
//


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