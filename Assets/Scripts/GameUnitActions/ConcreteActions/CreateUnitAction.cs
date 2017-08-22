using UnityEngine;

public class CreateUnitAction : AbstractRTSAction {

    private Identification.UnitType unitType;

    public CreateUnitAction(Identification.UnitType unitType, ActionData.SomeAction actionData) {
        this.unitType = unitType;
        this.actionData = actionData;
    }


//////////////////////////////////////////

    public override void Doing(ArmyStateData data) {

//        CreateUnitUsingTargetPoint(data);

        CreateUnitNearSelectedUnit(data);


    }

    private void CreateUnitNearSelectedUnit(ArmyStateData data) {
        Vector3 shiftVector = new Vector3(3, 3, 3);
        Vector3 targetPoint = data.SelectedUnits[0].Avatar.transform.position + shiftVector;


        data.ThisArmyManager.CreateWarrior(unitType, targetPoint);


        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }

    private void CreateUnitUsingTargetPoint(ArmyStateData data) {
        if (data.WaitingForTarget) {

            Debug.Log("CreateUnitAction: Waiting for target");

        } else {

            Debug.Log("Creating unit " + unitType.ToString() + ", target point is " + data.TargetPoint);

            if (data.TargetPointIsNowhere() && data.TargetUnit == null) {

                Debug.Log("No Target position or unit. Can't create a unit");

            } else {
                if (data.TargetPointIsNowhere()) {
                    data.TargetPoint = data.TargetUnit.Avatar.transform.position;
                }

                data.ThisArmyManager.CreateWarrior(unitType, data.TargetPoint);
            }

            data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);


        }
    }


    public override void Stopping(ArmyStateData data) {
//        data.WaitingForTarget = false;
        Debug.Log("Enter create unit state: " + unitType.ToString());
    }

    public override void Starting(ArmyStateData data) {
//        data.WaitingForTarget = true;
        Debug.Log("Exit create unit state: " + unitType.ToString());
    }

}