using System.Collections.Generic;
using UnityEngine;

public class PlayerController {



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

        ////////////////////////////////////////////////

        PlayerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("Unit is right clicked: " + unit.Description);
                }
        );

        PlayerArmyDispatcher.StartListening<Vector2>(ArmyMessageTypes.terrainIsLeftClicked,
                (Vector2 pos) => {
                    Debug.Log("Terrain is left clicked: " + pos);
                }
        );

        PlayerArmyDispatcher.StartListening<Vector3>(ArmyMessageTypes.terrainIsRightClicked,
                (Vector3 pos) => {
                    Debug.Log("Terrain is right clicked: " + pos);
                }
        );

        PlayerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("Unit is dead: " + unit.Description);
                }
        );



    } // subscribe on dispatcher events //


}