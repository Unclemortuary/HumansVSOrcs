using System;
using System.Collections.Generic;
using UnityEngine;


/**
 *  S - type of state
 *  T - type of transition between states
 */
public class StateMachine<S,T>  {

    private bool working = true;

    public void Stop()
    {
        working = false;
    }


    private S currentState;
    public S CurrentState {
        get {
            return currentState;
        }
        set {
            currentState = value;
        }
    }

    private Dictionary<S, IStateMachineState> nodesActions;
    private Dictionary<T, StateMachineTransition<S>> transitions;

    public void AddState(S stateType, IStateMachineState action) {
        if (nodesActions == null) {
            nodesActions = new Dictionary<S, IStateMachineState>();
        }

        nodesActions.Add(stateType, action);
    }

    public void AddTransition(T transitionType, StateMachineTransition<S> transition)  {
        if(transitions == null) {
            transitions = new Dictionary<T, StateMachineTransition<S>>();
        }

        transitions.Add(transitionType, transition);
    }



    public void ExecuteCurrentState() {
        nodesActions[currentState].Process();
    }


    public void Trigger(T transitionEvent) {
        StateMachineTransition<S> smt = transitions[transitionEvent];

        if (smt != null) {
//            if (smt.FromState == CurrentState) {
            if (smt.IsCurrent(CurrentState)) {
                smt.Action(smt.FromState, smt.ToState);
            }
            else
            {
                Debug.LogError("Can't make transition for event " + transitionEvent
                + " because current state is " + CurrentState);
            }
        }
        else
        {
            Debug.LogError("Cannot perform a transition. There is no transition for transitionEvent=" + transitionEvent);
        }
    }


/////////////////////



    protected void GoToState(S state) {
        CurrentState = state;

    }


//    private void SimpleTransition(S current, S next)
//    {
//        if(current != next)
//        {
//            currentState = next;
//        }
//    }


}