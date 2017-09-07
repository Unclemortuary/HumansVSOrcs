using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AIScript : ScriptableObject {

    [SerializeField]
    private int goodNumberOfWorkers = 2;
    public int GoodNumberOfWorkers {
        get {
            return  goodNumberOfWorkers;
        }
    }

    [SerializeField]
    private float timePerStep = 3f;
    public float TiimePerStep {
        get {
            return timePerStep;
        }
    }

    [SerializeField]
    private bool aiLightsOn;
    public bool AiLightsOn {
        get {
            return aiLightsOn;
        }
    }


    [SerializeField]
    private List<Identification.UnitType> buildingsPriorityOrder;
    public List<Identification.UnitType> BuildingsPriorityOrder {
        get {
            return buildingsPriorityOrder;
        }
    }


    [SerializeField]
    private List<Vector3> buildPoints;
    public List<Vector3> BuildPoints {
        get {
            return buildPoints;
        }
    }

    [SerializeField]
    private List<Vector3> mineBuildPoints;
    public List<Vector3> MineBuildPoints {
        get {
            return mineBuildPoints;
        }
    }

    [SerializeField]
    private List<Vector3> quarryBuildPoints;
    public List<Vector3> QuarryBuildPoints {
        get {
            return quarryBuildPoints;
        }
    }


    [SerializeField]
    private List<Vector3> attackPoints;
    public List<Vector3> AttackPoints {
        get {
            return attackPoints;
        }
    }



///////////////////////////////////////////////////////


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


    //////////////////////////////////////////////////////////////////


    [SerializeField]
    private List<CommandPair> commandsList;
    public List<CommandPair> CommandsList {
        get {
            return commandsList;
        }
    }


} // End of class //