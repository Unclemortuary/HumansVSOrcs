using System.Collections.Generic;
using UnityEngine;

public class ArmyStateData {

    private static Vector3 noWherePoint = new Vector3(-100,-100,-100);
    public bool IsNoWhere(Vector3 pos) {
        return pos == noWherePoint;
    }


    public ArmyManager ThisArmyManager { get; set; }

    public List<AbstractGameUnit> SelectedUnits { get; set; }

    public List<RTSActionType> AvailableActions { get; set; }

    public Vector3 TargetPoint { get; set; }

    public AbstractGameUnit TargetUnit { get; set; }

    public AbstractRTSAction CurrentRtsAction { get; set; }

    // ------------------------

    public ArmyStateData(ArmyManager armyManager) {
        this.ThisArmyManager = armyManager;

        SelectedUnits = new List<AbstractGameUnit>();
        AvailableActions = new List<RTSActionType>();

        ClearAll();
    }

    public void ClearAll() {
        SelectedUnits.Clear();
        AvailableActions.Clear();

        ClearTargetAndAction();
    }

    public void ClearTargetAndAction() {
        TargetPoint = noWherePoint;
        TargetUnit = null;
        CurrentRtsAction = null;
    }

}