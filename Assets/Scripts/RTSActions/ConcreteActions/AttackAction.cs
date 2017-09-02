using UnityEngine;

public class AttackAction : AbstractRTSAction {


    private float interval;

    public AttackAction(float interval) {
        this.interval = interval;
    }
//////////////////////////////////////

    public override void Doing(ArmyStateData data) {
        if (data.WaitingForTarget) {
//            Debug.Log("MoveToAction: Waiting for target");
        } else {
            string targetDescription = (data.TargetPoint!=null?data.TargetPoint.ToString():data.TargetUnit.Description);
            Debug.Log("Doing Attack! Target is " + targetDescription);



            float unitsNumber = data.SelectedUnits.Count;

            if (data.TargetPointIsNowhere()) {
                Debug.Log("Target unit is " + data.TargetUnit.Description);

                // Send all units to one target unit //
                for (int i = 0; i < unitsNumber; i++) {
                    AbstractGameUnit unit = data.SelectedUnits[i];

                    if (unit != null) {

                        data.ThisArmyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(
                                ArmyMessageTypes.unitCommandGoToAttackUnit, unit,
                                unit.ID
                        );
                    }
                }
            } else {
                // Target is a point

                Vector3 targetPoint = data.TargetPoint;

//              float squareSide = Mathf.Floor(Mathf.Sqrt(unitsNumber));
                float squareSide = Mathf.Ceil(Mathf.Sqrt(unitsNumber));

                float formationWidthNumber = squareSide;
                float formationLenNumber = Mathf.Ceil(unitsNumber / formationWidthNumber);

                float formationWidth = (formationWidthNumber - 1) * interval;
                float formationLen = (formationLenNumber - 1) * interval;

                float cornerX = targetPoint.x - 0.5f * formationWidth;
                float cornerZ = targetPoint.z - 0.5f * formationLen;


                // Send each unit to it's own destination //
                for (int i = 0; i < unitsNumber; i++) {
                    AbstractGameUnit unit = data.SelectedUnits[i];

                    if (unit != null) {

                        float z = i % formationLenNumber;
                        float x = Mathf.Floor(i / formationLenNumber);

                        Vector3 thisUnitTargetPoint = new Vector3(cornerX + x * interval, targetPoint.y, cornerZ + z * interval);
                        Debug.Log("this unit target=" + thisUnitTargetPoint);
                        Vector3 dest = thisUnitTargetPoint;


                        data.ThisArmyManager.Dispatcher.TriggerCommand<Vector3>(
                                ArmyMessageTypes.unitCommandGoToAttackPoint, dest,
                                unit.ID
                        );
                    }
                }

            }

            data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

        } // if not waiting for target //


    } // Doing() attack action //


    public override void Stopping(ArmyStateData data) {
        data.WaitingForTarget = false;
        Debug.Log("stopping Attack");
    }

    public override void Starting(ArmyStateData data) {
        data.WaitingForTarget = true;
        Debug.Log("starting Attack");
    }


}