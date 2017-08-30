using UnityEngine;
using UnityEngine.AI;

public class CreateUnitAction : AbstractRTSAction {

    private Identification.UnitType unitType;

    public CreateUnitAction(Identification.UnitType unitType) {
        this.unitType = unitType;
    }


//////////////////////////////////////////

    public override void Doing(ArmyStateData data) {

//        CreateUnitUsingTargetPoint(data);

        CreateUnitNearSelectedUnit(data);


    }

    private void CreateUnitNearSelectedUnit(ArmyStateData data) {
        Vector3 shiftVector = new Vector3(0f, 0, -19.5f);
        Vector3 targetPoint = data.SelectedUnits[0].Avatar.transform.position + shiftVector;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, 15.0f, NavMesh.AllAreas)) {
            targetPoint = hit.position;
        }

        AbstractGameUnit warrior = null;
        if (unitType == Identification.UnitType.Worker) {
            warrior = data.ThisArmyManager.CreateWorker(unitType, targetPoint);
        } else if (unitType == Identification.UnitType.FlyingWarrior) {
            warrior = data.ThisArmyManager.CreateFlyingWarrior(unitType, targetPoint);
        } else {
            warrior = data.ThisArmyManager.CreateWarrior(unitType, targetPoint);
        }

        targetPoint = targetPoint + 4 * shiftVector.normalized;
        if (NavMesh.SamplePosition(targetPoint, out hit, 5.0f, NavMesh.AllAreas)) {
            targetPoint = hit.position;
        }


//        targetPoint = new Vector3(0,0,0);
//        warrior.Avatar.GetComponent<NavMeshAgent>().destination = targetPoint;


        data.ThisArmyManager.Dispatcher.TriggerCommand<Vector3>(
                ArmyMessageTypes.unitCommandGoToPosition, targetPoint,
                warrior.ID
        );

        data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);

        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }




    public override void Stopping(ArmyStateData data) {
//        data.WaitingForTarget = false;
        Debug.Log("Enter create unit state: " + unitType.ToString());
    }

    public override void Starting(ArmyStateData data) {
//        data.WaitingForTarget = true;
//        data.ClearAll();

        Debug.Log("Exit create unit state: " + unitType.ToString());
    }

}