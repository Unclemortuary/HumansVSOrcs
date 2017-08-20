using System;
using UnityEngine;

public class ArmyStateMachine : StateMachine<ArmySMStateType, ArmySMTransitionType> {


    private ArmyStateData data;

    public ArmyStateMachine(ArmyStateData data) {

        this.data = data;

        InitializeStates();

        InitializeTransitions();

        GoToState(ArmySMStateType.freeState);


        SubscribeOnDispatcherMessages();

    }



    private void  SubscribeOnDispatcherMessages() {

        data.ThisArmyManager.Dispatcher.StartListening(ArmyMessageTypes.unitsSelected,
                () => {
                    Debug.Log("Dispatcher is working: message unitsSelected is sent");
                    if (CurrentState == ArmySMStateType.freeState) {
                        Trigger(ArmySMTransitionType.freeToSelected);
                    }
                });

        data.ThisArmyManager.Dispatcher.StartListening<AbstractRTSAction>(ArmyMessageTypes.actionMoveTo,
                (AbstractRTSAction action) => {
                    Debug.Log("Dispatcher performing message: Action-MoveTo");
                    if (CurrentState == ArmySMStateType.unitsSelected) {
                        data.CurrentRtsAction = action;
                        Trigger(ArmySMTransitionType.selectedToDoAction);
                    }
                });


    }



    private void InitializeStates() {
        AddState(ArmySMStateType.freeState,
                new SimpleActionSMState(() => {
                    Debug.Log("This is the freeState:" + data.ThisArmyManager.ArmyDescription);
                }));
        AddState(ArmySMStateType.unitsSelected,
                new SimpleActionSMState(() => {
                    Debug.Log("This is the unitsSelected:" + data.ThisArmyManager.ArmyDescription);
                }));
        AddState(ArmySMStateType.doAction, new PerformingRTSActionState(data));
    }


    private void InitializeTransitions() {
        AddTransition(ArmySMTransitionType.freeToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.freeState, ArmySMStateType.unitsSelected, SimpleTransition)
        );


        AddTransition(ArmySMTransitionType.doActionToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.unitsSelected, SimpleTransition
                )
        );
        AddTransition(ArmySMTransitionType.selectedToFree,
                new ArmyStateMachineTransition(
                        ArmySMStateType.unitsSelected, ArmySMStateType.freeState, SimpleTransition
                )
        );


        AddTransition(ArmySMTransitionType.selectedToDoAction,
                new ArmyStateMachineTransition(
                        ArmySMStateType.unitsSelected, ArmySMStateType.doAction, any2ActionTransition
                )
        );

        AddTransition(ArmySMTransitionType.doActionTofree,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.freeState, Action2AnyTransition
                )
        );
    }


////////////////////////////////////////////

    public void SimpleTransition(ArmySMStateType current, ArmySMStateType next) {
        if(current != next)
        {
            CurrentState = next;
        }
    }


//    public Action<ArmySMStateType, ArmySMStateType> SimpleTransition =
//        delegate(ArmySMStateType current, ArmySMStateType next) {
//        if(current != next)
//        {
//            CurrentState = next;
//        }
//    };


    public void any2ActionTransition(ArmySMStateType current, ArmySMStateType next) {
        if(current != next)
        {
            this.data.CurrentRtsAction.Starting(data);

            CurrentState = next;
        }
    }


    public void Action2AnyTransition(ArmySMStateType current, ArmySMStateType next) {
        if(current != next)
        {
            this.data.CurrentRtsAction.Stopping(data);

            CurrentState = next;
        }
    }




}