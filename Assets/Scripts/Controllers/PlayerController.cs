using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractContorller{



/////////////////////////////

    private ArmyManager armyManager;


    public ArmyDispatcher PlayerArmyDispatcher {
        get {
            return armyManager.Dispatcher;
        }
    }

    public PlayerController(ArmyManager armyManager) {
        SetArmyManager(armyManager);
    }

    public void SetArmyManager(ArmyManager manager) {
        this.armyManager = manager;
        SubscribeOnDspatcherEvents();
    }



    public void NewUnitsSelection(Bounds viewportBounds) {

        AbstractGameUnitsList selectedUnits = armyManager.FindWarriorsWithinViewportBounds(viewportBounds);

        Debug.Log("PlayerController: NewUnitsSelection");

        if (selectedUnits.Count > 0) {
            // Warriors were selected //

            PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
            PlayerArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, selectedUnits);


        } else {
            selectedUnits = armyManager.FindBuildingsWithinViewportBounds(viewportBounds);
            if (selectedUnits.Count > 0) {
                // Buildings were selected //

                PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
                PlayerArmyDispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.addBuildingToSelection, selectedUnits[0]);

            } else {

                PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
            }
        }

    }


    public void NewTargetUnit(AbstractGameUnit unit) {
        bool test = false;
        PlayerArmyDispatcher.TriggerFunc<bool>(ArmyMessageTypes.testWaitingForTarget, (bool v)=> { test = v; });
        if (test) {
            PlayerArmyDispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.setTargetUnit, unit);

            Debug.Log("Setting target unit: unitId=" + unit.ID);
        } else {
            Debug.Log("Click on unit means 'I want unit info'");

            /// Show enemy unit info ///////////////
            PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);

            armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(
                    ArmyMessageTypes.enemyUnitSelected, unit
            );

        } ///

    } // new target unit ///


    private void SubscribeOnDspatcherEvents() {

        PlayerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("trying to select single unit by leftclick");

                    AbstractGameUnitsList selectedUnit = new AbstractGameUnitsList();
                    selectedUnit.Add(unit);
                    PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
                    PlayerArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, selectedUnit);
                }
        );

        PlayerArmyDispatcher.StartListening<Vector3>(ArmyMessageTypes.terrainIsLeftClicked,
                (Vector3 pos) => {
                    bool test = false;
                    PlayerArmyDispatcher.TriggerFunc<bool>(ArmyMessageTypes.testWaitingForTarget, (bool v)=> { test = v; });
                    if (test) {
                        PlayerArmyDispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.setTargetPoint, pos);

                        Debug.Log("Setting target point: " + pos);
                    } else {
                        Debug.Log("Click on terrain means deselect");

                        PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
                    }
                }
        );


        ////////////////////////////////////////////////

        PlayerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("Unit is right clicked: " + unit.Description);
                }
        );

        PlayerArmyDispatcher.StartListening<Vector3>(ArmyMessageTypes.terrainIsRightClicked,
                (Vector3 pos) => {
                    Debug.Log("Terrain is right clicked: " + pos);

                    PerformDefaultAction(pos);
                }
        );

//        PlayerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
//                (AbstractGameUnit unit) => {
//                    Debug.Log("PlayerController:: Unit is dead: " + unit.Description);
//
//                    armyManager.DestroyGameUnit(unit.ID);
//                }
//        );
//
//

    } // subscribe on dispatcher events //


    private void PerformDefaultAction(Vector3 clicPosition) {
        PlayerArmyDispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction, RTSActionType.moveFormationTo5);
        PlayerArmyDispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.setTargetPoint, clicPosition);
    }


} // End of class //