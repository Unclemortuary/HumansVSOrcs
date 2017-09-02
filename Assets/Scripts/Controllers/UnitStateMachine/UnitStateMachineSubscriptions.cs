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
            stateMachine.GoToStandPreparedState();
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
                    stateMachine.GoToWalkingToPointState(pos);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
                () => {
                    stateMachine.GoToIdleState();
                },
                helper.ThisUnit.ID
        );



//////// builder ////////////////////////////////


        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandBuild,
                (AbstractGameUnit building) => {

                    stateMachine.GoToGoingToBuildState(building);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandCreateUnit,
                (AbstractGameUnit newUnit) => {

                    stateMachine.GoToCreatingUnitState(newUnit);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<float>(ArmyMessageTypes.unitCommandSetWorkDuration,
                (float time) => {
                    helper.TaskDuration = time;
                },
                helper.ThisUnit.ID
        );

////////// Attacking /////////////////////////////////

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToStandPreparedState,
                () => {
                    stateMachine.GoToStandPreparedState();
                },
                helper.ThisUnit.ID
        );


/////////////////////////////////

        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToAttackPoint,
                (Vector3 point) => {
                    stateMachine.GoToMoveAndAttackState(point);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandGoToAttackUnit,
                (AbstractGameUnit unit) => {
                    stateMachine.GoToFollowAndAttackState(unit);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToHoldPosition,
                () => {
                    stateMachine.GoToHoldPositionState();
                },
                helper.ThisUnit.ID
        );


    } // SubscribeOnDispatcherMessages() //




}