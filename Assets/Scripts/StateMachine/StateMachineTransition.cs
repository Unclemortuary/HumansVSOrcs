using System;

public class StateMachineTransition<S>  {

    protected S fromState;
    protected S toState;
    protected Action<S, S> action;

    public StateMachineTransition(S from, S to, Action<S, S> action) {
        this.fromState = from;
        this.toState = to;
        this.action = action;
    }

    public bool IsCurrent(S state) {
        return fromState.ToString() == state.ToString();
    }


    public S FromState {
        get {
            return fromState;
        }
    }

    public S ToState {
        get {
            return toState;
        }
    }

    public virtual Action<S, S> Action {
        get {
            return this.action;
        }
    }

}