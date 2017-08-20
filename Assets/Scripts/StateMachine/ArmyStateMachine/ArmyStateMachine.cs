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

        data.ThisArmyManager.Dispatcher.StartListening(ArmyMessageTypes.stopMachine,
                () => { Stop(); }
        );


        // Zaglushki //

        data.ThisArmyManager.Dispatcher.StartListening(ArmyMessageTypes.unitSelected,
                () => {
                    Debug.Log("Dispatcher is working: message unitsSelected is sent");
                    if (CurrentState == ArmySMStateType.freeState) {

                        Trigger(ArmySMTransitionType.freeToSelected);

                    } else if (CurrentState == ArmySMStateType.unitsSelected) {

                        // Select more units //

                    }
                });

        data.ThisArmyManager.Dispatcher.StartListening<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                (RTSActionType actionType) => {
                    Debug.Log("Dispatcher is sending message Action-" + actionType.ToString());

                    // Selected To DoAction //
                    if (CurrentState == ArmySMStateType.unitsSelected) {
                        data.CurrentRtsAction = GameManager.Instance.ActionsLibrary.GetRTSAction(actionType);
                        Trigger(ArmySMTransitionType.selectedToDoAction);
                    }
                });


    }// SubscribeOnDispatcherMessages() //



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

        AddTransition(ArmySMTransitionType.doActionToFree,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.freeState, Action2AnyTransition
                )
        );

        AddTransition(ArmySMTransitionType.doActionToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.unitsSelected, Action2AnyTransition
                )
        );

    } // InitializeTransitions() //


////////////////////////////////////////////

    public void SimpleTransition(ArmySMStateType current, ArmySMStateType next) {
        if(current != next)
        {
            CurrentState = next;
        }
    }



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