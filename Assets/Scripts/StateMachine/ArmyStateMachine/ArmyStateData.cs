using System.Collections.Generic;
using UnityEngine;

public class ArmyStateData {

    private static Vector3 noWherePoint = new Vector3(-100,-100,-100);
    public static bool IsNoWhere(Vector3 pos) {
        return pos == noWherePoint;
    }

    public bool TargetPointIsNowhere() {
        return IsNoWhere(this.TargetPoint);
    }

    //---------------------------

    public ArmyManager ThisArmyManager { get; set; }

    public AbstractGameUnitsList SelectedUnits { get; set; }

    public List<RTSActionType> AvailableActions { get; set; }

    public bool WaitingForTarget { get; set; }

    public Vector3 TargetPoint { get; set; }

    public AbstractGameUnit TargetUnit { get; set; }

    public AbstractRTSAction CurrentRtsAction { get; set; }


    // ------------------------

    public ArmyStateData(ArmyManager armyManager) {
        this.ThisArmyManager = armyManager;

        SelectedUnits = new AbstractGameUnitsList();
        AvailableActions = new List<RTSActionType>();

        ClearAll();
    }

    public void ClearAll() {
        SelectedUnits.Clear();
        AvailableActions.Clear();

        ClearTargetAndCurrentAction();
    }

    public void ClearTargetAndCurrentAction() {
        WaitingForTarget = true;
        TargetPoint = noWherePoint;
        TargetUnit = null;
        CurrentRtsAction = null;
    }

}