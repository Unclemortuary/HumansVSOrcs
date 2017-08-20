using UnityEngine;

public class MoveToAction : AbstractRTSAction {


    CommandPanelItem panelItem;

    public MoveToAction(CommandPanelItem panelItem) {
        this.panelItem = panelItem;
    }


    /////////////////////////////////////////////////////////////

    public override CommandPanelItem GetCommandPanelItem(ArmyStateData data) {
        return panelItem;
    }


    public override void Doing(ArmyStateData data) {
        Debug.Log("doing move, target point is " + data.TargetPoint);
    }


    public override void Stopping(ArmyStateData data) {
        Debug.Log("stopping move");
    }

    public override void Starting(ArmyStateData data) {
        Debug.Log("starting move");
    }
}