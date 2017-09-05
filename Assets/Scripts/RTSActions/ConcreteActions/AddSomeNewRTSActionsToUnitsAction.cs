using System.Collections.Generic;
using UnityEngine;

public class AddSomeNewRTSActionsToUnitsAction : AbstractRTSAction {

    private RTSActionType thisActionType;

    private Dictionary<Identification.UnitType, RTSActionType> actionsToAdd;


    public AddSomeNewRTSActionsToUnitsAction(RTSActionType thisActionType, Dictionary<Identification.UnitType,RTSActionType> actionsToAdd) {
        this.thisActionType = thisActionType;

        this.actionsToAdd = actionsToAdd;
    }

    public override void Doing(ArmyStateData data) {


// Pay the price //
        data.ThisArmyManager.AvailableResources.SpendResources(data.CurrentRtsAction.GetActionDataItem().PriceToUse);

        foreach(Identification.UnitType unitType in actionsToAdd.Keys) {
//            Debug.Log("AddSomeActions:: adding " + actionsToAdd[unitType].ToString() + " to " + unitType.ToString());
            GameManager.Instance.ArmyManagers[data.ThisArmyManager.ThisArmy].AddRTSActionToUnit(unitType, actionsToAdd[unitType]);
        }


        GameManager.Instance.ArmyManagers[data.ThisArmyManager.ThisArmy].RemoveRTSActionForEveryUnit(thisActionType);

        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }

    public override void Starting(ArmyStateData data) {
        Debug.Log(">>> AddTwoNewActionsAction:: starting");
    }

    public override void Stopping(ArmyStateData data) {
        Debug.Log(">>> AddTwoNewActionsAction:: stopping");
    }
}