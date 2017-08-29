using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingGamePositions : ScriptableObject{

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
    } // class GameUnitSpawnPoint //





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
    } // class ArmyResourcePair //

    [SerializeField]
    private List<ArmyResourcesPair> startingResources;
    public List<ArmyResourcesPair> StartingResources {
        get {
            return startingResources;
        }
    }


    [System.Serializable]
    public class CameraPosition {
        [SerializeField]
        private Identification.Army army;
        public Identification.Army Army {
            get {
                return army;
            }
        }
        [SerializeField]
        private Vector3 position;
        public Vector3 Position {
            get {
                return position;
            }
        }
    } // class CameraPosition //


    [SerializeField]
    private List<CameraPosition> cameraHolderPositioins;
    public List<CameraPosition> CameraHolderPositions {
        get {
            return cameraHolderPositioins;
        }
    }

} // End Of Class //


