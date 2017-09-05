using UnityEngine;

public class UnitStateMachineSubscriptions {

    private UnitStateMachineHelper helper;
    private ArmyManager armyManager;
    private UnitStateMachine stateMachine;

    public UnitStateMachineSubscriptions(UnitStateMachine machine, UnitStateMachineHelper helper, ArmyManager manager) {
        this.stateMachine = machine;
        this.helper = helper;
        this.armyManager = manager;

        SubscribeOnDispatcherMessages();
    }


    private void SubscribeOnDispatcherMessages() {

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToStandPreparedState, () => {
            stateMachine.TransitionToStandPreparedState();
        }, helper.ThisUnit.ID);


        armyManager.Dispatcher.StartListening<bool>(ArmyMessageTypes.unitCommandTurnSelection,
                (bool on) => {
                    helper.TurnSelection(on);
                },
                helper.ThisUnit.ID
        );


        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToPosition,
                (Vector3 pos) => {
                    Debug.Log("%% init go to point action %%");
                    stateMachine.TransitionToWalkingToPointState(pos);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
                () => {
                    stateMachine.TransitionToIdleState();
                },
                helper.ThisUnit.ID
        );



//////// builder ////////////////////////////////


        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandBuild,
                (AbstractGameUnit building) => {

                    stateMachine.TransitionToGoingToBuildState(building);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandCreateUnit,
                (AbstractGameUnit newUnit) => {

                    stateMachine.TransitionToCreatingUnitState(newUnit);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<float>(ArmyMessageTypes.unitCommandSetWorkDuration,
                (float time) => {
                    Debug.Log("USMS:: Setting TaskDuration = " + time);
                    helper.TaskDuration = time;
                },
                helper.ThisUnit.ID
        );

////////// Attacking /////////////////////////////////

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToStandPreparedState,
                () => {
                    stateMachine.TransitionToStandPreparedState();
                },
                helper.ThisUnit.ID
        );


/////////////////////////////////

        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToAttackPoint,
                (Vector3 point) => {
                    stateMachine.TransitionToMoveAndAttackState(point);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandGoToAttackUnit,
                (AbstractGameUnit unit) => {
                    stateMachine.TransitionToFollowAndAttackState(unit);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToHoldPosition,
                () => {
                    stateMachine.TransitionToHoldPositionState();
                },
                helper.ThisUnit.ID
        );


    } // SubscribeOnDispatcherMessages() //




}