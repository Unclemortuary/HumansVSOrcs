using System;

public class SMExecutorComponent<S,T> : RTSMonoBehaviour  {

    StateMachine<S,T> stateMachine;

    public void SetStateMachine(StateMachine<S,T> stateMachine) {
        this.stateMachine = stateMachine;
    }

    void Update() {
        stateMachine.ExecuteCurrentState();
    }
}