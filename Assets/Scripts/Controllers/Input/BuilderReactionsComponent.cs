using UnityEngine;
using UnityEngine.Networking;

public class BuilderReactionsComponent : RTSMonoBehaviour {


    UnitReactionsComponent reactionsComponent;

    public void Initialize(UnitReactionsComponent reactionsComponent) {

        this.reactionsComponent = reactionsComponent;

        SubscribeOnDispatcherMessages();
    }


    private void SubscribeOnDispatcherMessages() {

// Подписка на события здесь

//sdgf

//        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToPosition,
//                (Vector3 pos) => {
//                    reactionsComponent.Agent.destination = pos;
//                },
//                reactionsComponent.ThisUnit.ID
//        );
//
//        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
//                () => {
//                    reactionsComponent.Agent.ResetPath();
//                },
//                reactionsComponent.ThisUnit.ID
//        );
//

    } // Subscribe on dispatcher messages //


}

