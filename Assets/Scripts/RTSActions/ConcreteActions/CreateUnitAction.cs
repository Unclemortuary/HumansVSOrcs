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

        Debug.Log("Create Unit Action - Doing");

        CreateUnitNearSelectedUnit(data);

    }

    private void CreateUnitNearSelectedUnit(ArmyStateData data) {

        if (data.ThisArmyManager.AvailableResources.HaveFreeLivingPlaces()) {


            AbstractGameUnit performer = data.SelectedUnits[0];

//            Vector3 shiftVector = new Vector3(0f, 0, -19.5f);
            Vector3 shiftVector = - 0.7f * performer.Avatar.GetComponent<NavMeshObstacle>().size;
            shiftVector = new Vector3(shiftVector.x, 0, shiftVector.z);
            Debug.Log("shiftVector=" + shiftVector);

            Vector3 targetPoint = performer.Avatar.transform.position + shiftVector;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPoint, out hit, 5.0f, NavMesh.AllAreas)) {
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

            warrior.Avatar.SetActive(false);

            // Pay the price //
            data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);



            data.ThisArmyManager.Dispatcher.TriggerCommand<float>(
                    ArmyMessageTypes.unitCommandSetWorkDuration,
                    data.CurrentRtsAction.GetActionDataItem().TimeToComplete,
                    performer.ID);

            data.ThisArmyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(
                    ArmyMessageTypes.unitCommandCreateUnit, warrior, performer.ID);


        } // IF ( data.ThisArmyManager.AvailableResources.HaveFreeLivingPlaces() ) //

        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }




    public override void Stopping(ArmyStateData data) {
//        data.WaitingForTarget = false;
        Debug.Log("-= Exit create unit state: " + unitType.ToString());
    }

    public override void Starting(ArmyStateData data) {
//        data.WaitingForTarget = true;
//        data.ClearAll();

        Debug.Log("-= Enter create unit state: " + unitType.ToString());
    }

}