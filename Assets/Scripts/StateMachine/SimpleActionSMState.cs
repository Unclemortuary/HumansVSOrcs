using System;

public class SimpleActionSMState : IStateMachineState {

    private Action action;

    public SimpleActionSMState(Action action) {
        this.action = action;
    }


    public void Process() {
        action();
    }

}