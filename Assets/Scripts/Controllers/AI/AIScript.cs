using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AIScript : ScriptableObject {

    [System.Serializable]
    public enum CommandType {
        SELECT, // UnitTypesList
        DESELECN, //
        INVOKE_RTSACTION, // RTSActionType
        SET_TARGET_POINT, // Vector3
        SET_TARGET_UNIT, // AbstractGameUnit

        SELECT_UNIT_BY_TYPE, // UnitType
        SELECT_RANDOM_WARRIORS, // int
        ATTACK_RANDOM_ENEMY_UNIT,
        INVOKE_RANDOM_RTSACTION,
    }


    [System.Serializable]
    public class Arguments {
        [SerializeField]
        int numberOfUnitsToSelect;

        [SerializeField]
        private Identification.UnitType unitToSelectType;


//        [SerializeField]
//        AbstractGameUnitsList unitsToSelect;

        [SerializeField]
        RTSActionType actionToInvoke;

        [SerializeField]
        Vector3 targetPosition;

//        [SerializeField]
//        AbstractGameUnit  targetUnit;

    }

    [System.Serializable]
    public class CommandPair {
        [SerializeField]
        private CommandType command;
        [SerializeField]
        private Arguments args;
    }


    [SerializeField]
    private List<CommandPair> commandsList;
    public List<CommandPair> CommandsList {
        get {
            return commandsList;
        }
    }


    [SerializeField]
    private List<Identification.UnitType> buildingsPriorityOrder;


    [SerializeField]
    private List<Vector3> attackPoints;



}