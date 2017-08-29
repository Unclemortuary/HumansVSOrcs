using UnityEngine;
using UnityEngine.Networking;

public class BuilderReactionsComponent : RTSMonoBehaviour {


    private float workingRadius = 225; //15;
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
                },
                reactionsComponent.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<float>(ArmyMessageTypes.unitCommandSetWorkDuration,
                (float time) => {
                    workTaskDuration = time;
                },
                reactionsComponent.ThisUnit.ID
        );

//        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
//                () => {
//                    reactionsComponent.Agent.ResetPath();
//                },
//                reactionsComponent.ThisUnit.ID
//        );
//

    } // Subscribe on dispatcher messages //


    void Update() {
        if (goingToBuild) {
            if ((transform.position - targetBuilding.Avatar.transform.position).sqrMagnitude > workingRadius) {
                Debug.Log("distance=" + (transform.position - targetBuilding.Avatar.transform.position).magnitude);
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
                    targetBuilding.IsActive = true;
                }, workTaskDuration);
//                }, data.CurrentRtsAction.GetActionDataItem().TimeToComplete);

                this.reactionsComponent.Agent.destination = transform.position;
                goingToBuild = false;
            }
        } // if (going to build) //
    }

}

