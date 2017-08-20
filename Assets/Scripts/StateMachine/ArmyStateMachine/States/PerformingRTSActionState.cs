public class PerformingRTSActionState : IStateMachineState {

    ArmyStateData data;

    public PerformingRTSActionState(ArmyStateData data) {
        this.data = data;
    }


    public void Process() {
        data.CurrentRtsAction.Doing(data);
    }


}