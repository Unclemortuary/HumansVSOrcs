using UnityEngine;

public class MoveFormationToAction : AbstractRTSAction {

    private float interval;

    public MoveFormationToAction(float interval) {
        this.interval = interval;
    }
//////////////////////////////////////

    public override void Doing(ArmyStateData data) {
        if (data.WaitingForTarget) {
//            Debug.Log("MoveToAction: Waiting for target");
        } else {
            Debug.Log("doing move, target point is " + data.TargetPoint);

            Vector3 targetPoint = data.TargetPoint;
            if (targetPoint == null) {
                targetPoint = data.TargetUnit.Avatar.transform.position;
            }

            float unitsNumber = data.SelectedUnits.Count;

//            float squareSide = Mathf.Floor(Mathf.Sqrt(unitsNumber));
            float squareSide = Mathf.Ceil(Mathf.Sqrt(unitsNumber));

            float formationWidthNumber = squareSide;
            float formationLenNumber = Mathf.Ceil(unitsNumber / formationWidthNumber);

            float formationWidth = (formationWidthNumber - 1) * interval;
            float formationLen = (formationLenNumber - 1) * interval;

            float cornerX = targetPoint.x - 0.5f * formationWidth;
            float cornerZ = targetPoint.z - 0.5f * formationLen;

            Debug.Log("targetPoint = " + targetPoint +
                ", formationLen = " + formationLenNumber + "x" + interval + "=" + formationLen +
                ", formationWidth = " + formationWidthNumber + "x" + interval + "=" + formationWidth +
                ", corner=(" + cornerX + "," + cornerZ + ")"
            );

            // Send each unit to it's own destination //
            for (int i = 0; i < unitsNumber; i++) {
                AbstractGameUnit unit = data.SelectedUnits[i];

                if (unit != null) {

                    float z = i % formationLenNumber;
                    float x = Mathf.Floor(i / formationLenNumber);

//                    float z = Mathf.Floor(i / formationLenNumber);
//                    float x = i - z * formationLenNumber;
//
                    Vector3 thisUnitTargetPoint = new Vector3(cornerX + x * interval, targetPoint.y, cornerZ + z * interval);
                    Debug.Log("this unit target=" + thisUnitTargetPoint);
                    Vector3 dest = thisUnitTargetPoint;
                    data.ThisArmyManager.Dispatcher.TriggerCommand<Vector3>(
                            ArmyMessageTypes.unitCommandGoToPosition, dest,
                            unit.ID
                    );
                }
            }

//            foreach (AbstractGameUnit unit in data.SelectedUnits) {
//                data.ThisArmyManager.Dispatcher.TriggerCommand<Vector3>(
//                        ArmyMessageTypes.unitCommandGoToPosition, data.TargetPoint,
//                        unit.ID
//                );
//            }



            data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);

        }


    }


    public override void Stopping(ArmyStateData data) {
        data.WaitingForTarget = false;
        Debug.Log("stopping Move Formation To");
    }

    public override void Starting(ArmyStateData data) {
        data.WaitingForTarget = true;
        Debug.Log("starting Move Formation To");
    }

}