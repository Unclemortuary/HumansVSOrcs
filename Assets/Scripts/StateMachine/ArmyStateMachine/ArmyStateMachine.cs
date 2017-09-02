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


        data.ThisArmyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.excludeUnitFromSelection,
                (AbstractGameUnit unit) => {
                    if (data.SelectedUnits.Contains(unit)) {
                        data.SelectedUnits.Remove(unit);
                        updateSelected();
                    }
                }
        );


        data.ThisArmyManager.Dispatcher.StartListening(ArmyMessageTypes.deselectUnits,
                () => {
                    Debug.Log("StateMachine: deselect all");

                    if (CurrentState == ArmySMStateType.unitsSelected) {

                        Trigger(ArmySMTransitionType.selectedToFree);

                    } else if (CurrentState == ArmySMStateType.doAction) {

                        Trigger(ArmySMTransitionType.doActionToFree);

                    }

                    updateSelected();

                });


        data.ThisArmyManager.Dispatcher.StartListening<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection,
                (AbstractGameUnitsList list) => {
                    Debug.Log("StateMachine: selecting units");

                    if (CurrentState == ArmySMStateType.freeState) {

                        data.SelectedUnits = list;
                        TurnAllSelections(true);

                        Trigger(ArmySMTransitionType.freeToSelected);


                    } else if (CurrentState == ArmySMStateType.unitsSelected) {

                        if (data.SelectedUnits == null) {
                            data.SelectedUnits = list;
                        } else {
                            data.SelectedUnits.AddRange(list);
                        }
                        TurnAllSelections(true);

                    } else if (CurrentState == ArmySMStateType.doAction) {

                        if (data.SelectedUnits == null) {
                            data.SelectedUnits = list;
                        } else {
                            data.SelectedUnits.AddRange(list);
                        }
                        TurnAllSelections(true);

                        Trigger(ArmySMTransitionType.doActionToSelected);
                    }

                    updateSelected();

                });

        data.ThisArmyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.addBuildingToSelection,
                (AbstractGameUnit unit) => {
                    Debug.Log("StateMachine: selecting a building");

                    if (CurrentState == ArmySMStateType.freeState) {

                        data.SelectedUnits = new AbstractGameUnitsList();
                        data.SelectedUnits.Add(unit);
                        TurnAllSelections(true);

                        Trigger(ArmySMTransitionType.freeToSelected);


                    } else if (CurrentState == ArmySMStateType.unitsSelected) {

                        data.SelectedUnits = new AbstractGameUnitsList();
                        data.SelectedUnits.Add(unit);
                        TurnAllSelections(true);

                    } else if (CurrentState == ArmySMStateType.doAction) {

                        data.SelectedUnits = new AbstractGameUnitsList();
                        data.SelectedUnits.Add(unit);
                        TurnAllSelections(true);

                        Trigger(ArmySMTransitionType.doActionToSelected);
                    }

                    updateSelected();

                });


        data.ThisArmyManager.Dispatcher.StartListeningFunc<bool>(ArmyMessageTypes.testWaitingForTarget, () => {
            return data.WaitingForTarget;
        });

        data.ThisArmyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.setTargetPoint,
                (Vector3 point) => {
                    Debug.Log("StateMachine: Setting Target Point" + point);

                    data.TargetPoint = point;
                    data.WaitingForTarget = false;
                });

        data.ThisArmyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.setTargetPoint,
                (AbstractGameUnit unit) => {
                    Debug.Log("StateMachine: Setting Target Unit" + unit.ID);

                    data.TargetUnit = unit;
                    data.WaitingForTarget = false;
                });


// #######################################################

        data.ThisArmyManager.Dispatcher.StartListening<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
                (RTSActionType actionType) => {
                    Debug.Log("Dispatcher is sending message Action-" + actionType.ToString());

                    // Selected To DoAction //
                    if (CurrentState == ArmySMStateType.unitsSelected) {
                        Debug.Log("Previous state was selected");
                        data.CurrentRtsAction = GameManager.Instance.ActionsLibrary.GetRTSAction(actionType);
                        Trigger(ArmySMTransitionType.selectedToDoAction);

                    } else if (CurrentState == ArmySMStateType.doAction) {
                        Debug.Log("Previous state was doAction");

                        Trigger(ArmySMTransitionType.doActionToSelected);
                        data.CurrentRtsAction = GameManager.Instance.ActionsLibrary.GetRTSAction(actionType);
                        Trigger(ArmySMTransitionType.selectedToDoAction);

                    } else if (CurrentState == ArmySMStateType.freeState) {

                        Debug.Log("Previous state was freestate, Can't perform action with no selected units");

                        // Actions whith no performer??? //

                    }
                });


    }// SubscribeOnDispatcherMessages() //



    private void InitializeStates() {
        AddState(ArmySMStateType.freeState,
                new SimpleActionSMState(() => {
//                    Debug.Log("This is the freeState:" + data.ThisArmyManager.ArmyDescription);
                }));
        AddState(ArmySMStateType.unitsSelected,
                new SimpleActionSMState(() => {
//                    Debug.Log("This is the unitsSelected:" + data.ThisArmyManager.ArmyDescription);
                }));
        AddState(ArmySMStateType.doAction, new PerformingRTSActionState(data));
    }


    ///////////////////////////////////////////////////////////////

    private void InitializeTransitions() {
        AddTransition(ArmySMTransitionType.freeToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.freeState, ArmySMStateType.unitsSelected, Free2SelectedTransition
                )
        );


        AddTransition(ArmySMTransitionType.selectedToFree,
                new ArmyStateMachineTransition(
                        ArmySMStateType.unitsSelected, ArmySMStateType.freeState, Selected2FreeTransition
                )
        );

        AddTransition(ArmySMTransitionType.selectedToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.unitsSelected, ArmySMStateType.unitsSelected, Selected2SelectedTransition
                )
        );


        AddTransition(ArmySMTransitionType.selectedToDoAction,
                new ArmyStateMachineTransition(
                        ArmySMStateType.unitsSelected, ArmySMStateType.doAction, Selected2ActionTransition
                )
        );

//        AddTransition(ArmySMTransitionType.doActionToDoAction,
//                new ArmyStateMachineTransition(
//                        ArmySMStateType.doAction, ArmySMStateType.doAction, Action2ActionTransition
//                )
//        );

        AddTransition(ArmySMTransitionType.doActionToFree,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.freeState, Action2FreeTransition
                )
        );

        AddTransition(ArmySMTransitionType.doActionToSelected,
                new ArmyStateMachineTransition(
                        ArmySMStateType.doAction, ArmySMStateType.unitsSelected, Action2SelectedTransition
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

    private void updateSelected() {
        data.ThisArmyManager.Dispatcher.TriggerCommand<AbstractGameUnitsList>(
                        ArmyMessageTypes.selectionChanged, data.SelectedUnits
        );
    }

    private void TurnAllSelections(bool on) {
        foreach (AbstractGameUnit unit in data.SelectedUnits) {
            data.ThisArmyManager.Dispatcher.TriggerCommand<bool>(
                    ArmyMessageTypes.unitCommandTurnSelection,
                    on, unit.ID);
        }
    }

    /// *** /// ************************************************************************************ ///

    public void Free2SelectedTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  free2selected");
        if(current != next)
        {
            TurnAllSelections(true);
            CurrentState = next;
        }
    }

    public void Selected2SelectedTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  selected2selected");
        TurnAllSelections(true);
    }

    public void Selected2FreeTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  selected2free");
        if(current != next)
        {
            TurnAllSelections(false);
            data.ClearAll();
            CurrentState = next;
        }
    }



    public void Selected2ActionTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  selected2action");
        if(current != next)
        {
            GameResources price = this.data.CurrentRtsAction.GetActionDataItem().PriceToUse;

            if (this.data.ThisArmyManager.AvailableResources.HaveEnoughResources(price)) {
                this.data.CurrentRtsAction.Starting(data);

                CurrentState = next;
            } else {
                Debug.Log("Not enough game resources");
            }

        }
    }


//    public void Action2ActionTransition(ArmySMStateType current, ArmySMStateType next) {
//
//        this.data.CurrentRtsAction.Starting(data);
//
//        CurrentState = next;
//    }


    public void Action2FreeTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  action2free");
        if(current != next)
        {
            this.data.CurrentRtsAction.Stopping(data);

            TurnAllSelections(false);
            data.ClearAll();

            CurrentState = next;
        }
    }


    public void Action2SelectedTransition(ArmySMStateType current, ArmySMStateType next) {
        Debug.Log(">>>  action2selected");
        if(current != next)
        {
            this.data.CurrentRtsAction.Stopping(data);

            data.ClearTargetAndCurrentAction();

            CurrentState = next;
        }
    }




}