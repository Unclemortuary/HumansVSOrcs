using System.Collections.Generic;
using UnityEngine;

public class ArmyManager {

    private CommonGameUnitFactory warriorFactory;
//    public AbstractWarriorFactory WarriorFactory {
//        get {
//            return warriorFactory;
//        }
//    }

    private CommonGameUnitFactory buildingFactory;
//    public AbstractBuildingFactory BuildingFactory {
//        get {
//            return buildingFactory;
//        }
//    }



    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! *************** //
//    private Controller controller;

    private string description;
    public string ArmyDescription {
        get {
            return description;
        }
        set {
            description = value;
        }
    }

    private Identification.Army thisArmy;

    private Dictionary<int, AbstractGameUnit> warriors;
    private Dictionary<int, AbstractGameUnit> buildings;

    private int nextID;




///////////////////////////////////////////////////////////////////////////////
///// PUBLIC //////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////


    public AbstractGameUnit CreateWarrior(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = null;
        newUnit = warriorFactory.CreateGameUnit(type, position);

//        switch (type) {
//            case Identification.UnitType.Archer:
//                newUnit = warriorFactory.CreateArcher(position);
//            break;
//            case Identification.UnitType.Swordsman:
//                newUnit = warriorFactory.CreateSwordsman(position);
//            break;
//            case Identification.UnitType.Horseman:
//                newUnit = warriorFactory.CreateHorseman(position);
//            break;
//        }

        newUnit.ID = nextID;
        newUnit.Description = "Soldier - " + type.ToString() + " of " + ArmyDescription;

        GameUnitID unitId = newUnit.Avatar.AddComponent<GameUnitID>();
        unitId.PersonalID = newUnit.ID;
        unitId.Army = thisArmy;

        warriors.Add(nextID, newUnit);


        nextID++;

        return newUnit;
    }


    public AbstractGameUnit CreateBuilding(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = null;
        newUnit = buildingFactory.CreateGameUnit(type, position);

        newUnit.ID = nextID;
        newUnit.Description = "Building - " + type.ToString() + " of " + ArmyDescription;

        buildings.Add(nextID, newUnit);

        GameUnitID unitId = newUnit.Avatar.AddComponent<GameUnitID>();
        unitId.PersonalID = newUnit.ID;
        unitId.Army = thisArmy;

        nextID++;

        return newUnit;
    }


    public AbstractGameUnit FindGameUnit(int id) {
        if (warriors.ContainsKey(id)) {
            return warriors[id];
        }
        if (buildings.ContainsKey(id)) {
            return buildings[id];
        }

        return null;
    }


    public ArmyManager(Identification.Army army, int startingID, CommonGameUnitFactory warriorFactory,
            CommonGameUnitFactory buildingFactory /*, Controller ctrlr */) {

        warriors = new Dictionary<int, AbstractGameUnit>();
        buildings = new Dictionary<int, AbstractGameUnit>();

        this.thisArmy = army;
        this.description = army.ToString() + " army";
        this.nextID = startingID;
        this.warriorFactory = warriorFactory;
        this.buildingFactory = buildingFactory;
//        this.controller = ctrlr;
    }



}
