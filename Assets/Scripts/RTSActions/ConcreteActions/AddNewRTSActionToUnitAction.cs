using UnityEngine;

public class AddNewRTSActionToUnitAction : AbstractRTSAction {

    private Identification.UnitType unitType;
    private RTSActionType actionType;

    public AddNewRTSActionToUnitAction(Identification.UnitType unitType, RTSActionType actionType) {
        this.unitType = unitType;
        this.actionType = actionType;
    }

    public override void Doing(ArmyStateData data) {


        // Pay the price //
        data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);

        GameManager.Instance.ArmyManagers[data.ThisArmyManager.ThisArmy].AddRTSActionToUnit(unitType, actionType);

        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }

    public override void Starting(ArmyStateData data) {
        Debug.Log(">>> AddNewActionAction:: starting");
    }

    public override void Stopping(ArmyStateData data) {
        Debug.Log(">>> AddNewActionAction:: stopping");
    }
}