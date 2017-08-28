using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingGameUnitsPositions : ScriptableObject{

    [System.Serializable]
    public class GameUnitSpawnPoint {
        [SerializeField]
        private Identification.UnitType unitType;
        public Identification.UnitType UnitType {
            get {
                return unitType;
            }
        }

        [SerializeField]
        private Identification.Army armyType;
        public Identification.Army ArmyType {
            get {
                return armyType;
            }
        }

        [SerializeField]
        private Vector3 position;
        public Vector3 Position {
            get {
                return position;
            }
        }
    }

    [SerializeField]
    private List<GameUnitSpawnPoint> units;

    [SerializeField]
    private List<GameUnitSpawnPoint> buildings;


    public List<GameUnitSpawnPoint> GetUnitsPositions() {
        return units;
    }

    public List<GameUnitSpawnPoint> GetBuildingsPositions() {
        return buildings;
    }


    [System.Serializable]
    public class ArmyResourcesPair {
        [SerializeField]
        private Identification.Army armyType;
        public Identification.Army ArmyType {
            get {
                return armyType;
            }
        }
        [SerializeField]
        private GameResources resourcesAmount;
        public GameResources ResourcesAmount {
            get {
                return resourcesAmount;
            }
        }
    }

    [SerializeField]
    private List<ArmyResourcesPair> startingResources;
    public List<ArmyResourcesPair> StartingResources {
        get {
            return startingResources;
        }
    }
}

