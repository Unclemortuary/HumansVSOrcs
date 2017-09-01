using UnityEngine;
using UnityEngine.AI;

public class BuilderReactionsComponent : RTSMonoBehaviour {


    private float workingRadius = 225; //15;
    private float delta = 3;

    private AbstractGameUnit targetBuilding;

    private bool goingToBuild = false;

    private float epsilon = 0.001f;

    private float workTaskDuration = 1;

    UnitReactionsComponent reactionsComponent;

    public void Initialize(UnitReactionsComponent reactionsComponent) {

        this.reactionsComponent = reactionsComponent;

        SubscribeOnDispatcherMessages();
    }


    private void SubscribeOnDispatcherMessages() {

// Подписка на события здесь

//sdgf

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandBuild,
                (AbstractGameUnit building) => {
                    targetBuilding = building;
                    goingToBuild = true;

                    SetActivityTo(false);

                    ///////// Adjast miniimum distance to build ////////
                    Vector3 buildingExtents = targetBuilding.Avatar.GetComponent<Collider>().bounds.extents;
                    workingRadius = buildingExtents.x * buildingExtents.x + buildingExtents.z * buildingExtents.z;

                    Vector3 obstacleSize = targetBuilding.Avatar.GetComponent<NavMeshObstacle>().size;
                    float navmeshRadius = obstacleSize.x * obstacleSize.x * 0.25f
                                        + obstacleSize.z * obstacleSize.z * 0.25f;

//                    navmeshRadius = navmeshRadius * navmeshRadius;
                    if(workingRadius < navmeshRadius) {
                        workingRadius = navmeshRadius;
                    }

                    workingRadius += reactionsComponent.StoppingDistance + delta;
                    //////////////////////////////////////////////////////

                },
                reactionsComponent.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<float>(ArmyMessageTypes.unitCommandSetWorkDuration,
                (float time) => {
                    workTaskDuration = time;
                },
                reactionsComponent.ThisUnit.ID
        );



    } // Subscribe on dispatcher messages //


    void Update() {
        if (goingToBuild) {
            if ((transform.position - targetBuilding.Avatar.transform.position).sqrMagnitude > workingRadius) {
                Debug.Log("distance^2=" + (transform.position - targetBuilding.Avatar.transform.position).sqrMagnitude +
                        ", buildingDistance^2=" + workingRadius);
                this.reactionsComponent.Agent.destination = targetBuilding.Avatar.transform.position;
            } else {
//                targetBuilding.

                targetBuilding.Avatar.GetComponent<BuildingComponent>().SetTransparent(false);

                GameObject scaffold = targetBuilding.Avatar.transform.Find("BuildingBuild").gameObject;


                if (scaffold != null) {
                    Debug.Log("scaffold is found");
                } else {
                    Debug.Log("scaffold is not found");
                }

                Debug.Log("Setting scaffold active");
                scaffold.SetActive(true);



                new Timer(targetBuilding.Avatar, delegate  {
                    scaffold.SetActive(false);


                    SetActivityTo(true);


                    ////////////////////////////////////
                    Identification.UnitType type = armyManager.GetBuildingTypeByAbstractGameUnit(targetBuilding);
                    if (type == Identification.UnitType.GeneralHouse) {
                        armyManager.AvailableResources.ChangeResourceAmount(
                                    GameResources.ResourceType.GENERAL_HOUSES, 1);

                    } else if (type == Identification.UnitType.SimpleHouse) {
                        armyManager.AvailableResources.ChangeResourceAmount(
                                    GameResources.ResourceType.LIVING_HOUSES, 1);
                    }
                    ///////////////////////////////////

                }, workTaskDuration);
//                }, data.CurrentRtsAction.GetActionDataItem().TimeToComplete);

                this.reactionsComponent.Agent.destination = transform.position;
                goingToBuild = false;
            }
        } // if (going to build) //
    }


    private void SetActivityTo(bool val) {
        targetBuilding.IsActive = val;
        reactionsComponent.ThisUnit.IsActive = val;
        armyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
    }


} // End of class //

