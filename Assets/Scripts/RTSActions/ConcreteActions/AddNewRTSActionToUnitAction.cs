using UnityEngine;

public class AddNewRTSActionToUnitAction : AbstractRTSAction {

    private RTSActionType thisActionType;

    private Identification.UnitType unitType;
    private RTSActionType actionType;

    public AddNewRTSActionToUnitAction(RTSActionType thisActionType, Identification.UnitType unitType, RTSActionType actionType) {
        this.thisActionType = thisActionType;

        this.unitType = unitType;
        this.actionType = actionType;
    }

    public override void Doing(ArmyStateData data) {


        // Pay the price //
        data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);

        GameManager.Instance.ArmyManagers[data.ThisArmyManager.ThisArmy].AddRTSActionToUnit(unitType, actionType);
        GameManager.Instance.ArmyManagers[data.ThisArmyManager.ThisArmy].RemoveRTSActionForEveryUnit(thisActionType);

        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }

    public override void Starting(ArmyStateData data) {
        Debug.Log(">>> AddNewActionAction:: starting");
    }

    public override void Stopping(ArmyStateData data) {
        Debug.Log(">>> AddNewActionAction:: stopping");
    }
}