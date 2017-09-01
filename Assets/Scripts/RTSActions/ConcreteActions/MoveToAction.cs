using UnityEngine;

public class MoveToAction : AbstractRTSAction {


/////////////////////////////////////////////////////////////


    public override void Doing(ArmyStateData data) {

        if (data.SelectedUnits.Count > 0) {
            if (data.WaitingForTarget) {
                Debug.Log("MoveToAction: Waiting for target");
            } else {
                Debug.Log("doing move, target point is " + data.TargetPoint);

                if (data.TargetUnit == null) {
                    foreach (AbstractGameUnit unit in data.SelectedUnits) {
                        data.ThisArmyManager.Dispatcher.TriggerCommand<Vector3>(
                                ArmyMessageTypes.unitCommandGoToPosition, data.TargetPoint,
                                unit.ID
                        );
                    }


                } else {
//a;sldkjff;alksdjf;lakjsd;flkja
                }

                data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

            }
        } else {
            // No Selected Units //
            data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
        }



    } // Doing //


    public override void Stopping(ArmyStateData data) {
        data.WaitingForTarget = false;
        Debug.Log("stopping move");
    }

    public override void Starting(ArmyStateData data) {
        data.WaitingForTarget = true;
        Debug.Log("starting move");
    }

}