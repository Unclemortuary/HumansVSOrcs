using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class ArmyManager {

    private CommonGameUnitFactory warriorFactory;

    private CommonGameUnitFactory buildingFactory;

    private ArmyDispatcher dispatcher;
    public ArmyDispatcher Dispatcher {
        get {
            return dispatcher;
        }
    }

    private ArmyStateMachine stateMachine;
    public ArmyStateMachine StateMachine {
        get {
            return stateMachine;
        }
    }


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
    public Identification.Army ThisArmy {
        get {
            return thisArmy;
        }
    }

    private int nextID;

    private Dictionary<int, AbstractGameUnit> warriors;
    private Dictionary<int, AbstractGameUnit> buildings;


    private string GenerateDescription(Identification.UnitType type, AbstractGameUnit unit) {
        StringBuilder sb = new StringBuilder();

        sb.Append("Unit of type ").Append(type.ToString());
        sb.Append(" in army of " + thisArmy.ToString());

        return sb.ToString();
    }

    public GameObject CreateBuildingGhost(Identification.UnitType type, Vector3 position) {
        GameObject go = buildingFactory.CreateGameUnit(type, position).Avatar;
        go.AddComponent<BuildingComponent>();
        return go;
    }

    private AbstractGameUnit CreateUnit(CommonGameUnitFactory factory, Identification.UnitType type,
            Vector3 position, Dictionary<int, AbstractGameUnit> unitsStorage) {

        AbstractGameUnit newUnit = factory.CreateGameUnit(type, position);

        newUnit.Description = GenerateDescription(type, newUnit);

        AssimilateUnit(newUnit, unitsStorage);

        return newUnit;
    }

    private void AssimilateUnit(AbstractGameUnit newUnit, Dictionary<int, AbstractGameUnit> unitsStorage) {

        newUnit.ID = nextID;

        GameUnitID unitId = newUnit.Avatar.AddComponent<GameUnitID>();
        unitId.PersonalID = newUnit.ID;
        unitId.Army = thisArmy;

        nextID++;

        unitsStorage.Add(newUnit.ID, newUnit);
    }


    private void RemoveOldIDComponent(AbstractGameUnit unit) {
        GameUnitID oldID = unit.Avatar.GetComponent<GameUnitID>();
        if (oldID != null) {
            GameObject.Destroy(oldID);
        }
    }

///////////////////////////////////////////////////////////////////////////////
///// PUBLIC //////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////


    public AbstractGameUnit CreateWarrior(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = CreateUnit(warriorFactory, type, position, warriors);

        UnitReactionsComponent reactionsComponent = newUnit.Avatar.AddComponent<UnitReactionsComponent>();
        reactionsComponent.SetGameUnit(newUnit);

        NavMeshAgent agent = newUnit.Avatar.AddComponent<NavMeshAgent>();
        reactionsComponent.SetNavmeshAgent(agent);

        return newUnit;
    }


    public AbstractGameUnit CreateBuilding(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = CreateUnit(buildingFactory, type, position, buildings);

        UnitReactionsComponent reactionsComponent = newUnit.Avatar.AddComponent<UnitReactionsComponent>();
        reactionsComponent.SetGameUnit(newUnit);

        NavMeshObstacle obstacle = newUnit.Avatar.AddComponent<NavMeshObstacle>();
        obstacle.carving = true;
//        reactionsComponent.SetNavmeshAgent(agent);



        // This is a building //
        newUnit.Avatar.AddComponent<BuildingComponent>();

        return newUnit;
    }



    /* Join neutral or enemy warrior to this army */
    public void JoinWarrior(AbstractGameUnit unit) {
        RemoveOldIDComponent(unit);

        AssimilateUnit(unit, warriors);
    }


    public void JoinBuilding(AbstractGameUnit unit) {
        RemoveOldIDComponent(unit);

        AssimilateUnit(unit, buildings);

        // This is a building //
        unit.Avatar.AddComponent<BuildingComponent>();
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

    public void DestroyGameUnit(int id) {
        AbstractGameUnit unit = null;
        if (warriors.ContainsKey(id)) {
            unit =  warriors[id];
            warriors.Remove(id);
        }
        if (buildings.ContainsKey(id)) {
            unit =  buildings[id];
            buildings.Remove(id);
        }

        if (unit != null) {
//            GameObject.Destroy(unit.Avatar);
        }
    }




    public AbstractGameUnitsList FindWarriorsWithinViewportBounds(Bounds bounds) {
        return FindGameUnitsWithinViewportBounds(warriors.Values, bounds);
    }

    public AbstractGameUnitsList FindBuildingsWithinViewportBounds(Bounds bounds) {
        return FindGameUnitsWithinViewportBounds(buildings.Values, bounds);
    }

    public AbstractGameUnitsList FindGameUnitsWithinViewportBounds(IEnumerable<AbstractGameUnit> enumerable, Bounds bounds) {
        AbstractGameUnitsList selectedUnits = new AbstractGameUnitsList();

        foreach (AbstractGameUnit unit in enumerable) {
            if (IsWithinSelectionBounds(unit.Avatar, bounds)) {
                selectedUnits.Add(unit);
            }
        }

        return selectedUnits;
    }

//    public List<AbstractGameUnit> FindGameUnitsWithinViewportBounds(Dictionary<int, AbstractGameUnit> dict, Bounds bounds) {
//        List<AbstractGameUnit> selectedUnits = new List<AbstractGameUnit>();
//
//        foreach (AbstractGameUnit unit in dict.Values) {
//            if (IsWithinSelectionBounds(unit.Avatar, bounds)) {
//                selectedUnits.Add(unit);
//            }
//        }
//
//        return selectedUnits;
//    }

    public bool IsWithinSelectionBounds(GameObject gameObject, Bounds bounds)
    {
        // !!! Camera.main !!! // Dependence to be removed //
        return bounds.Contains(Camera.main.WorldToViewportPoint( gameObject.transform.position ));
    }



////////////////////////////////////////////////////////////////////////
    /// Constructor ////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    public ArmyManager(Identification.Army army, int startingID, CommonGameUnitFactory warriorFactory,
            CommonGameUnitFactory buildingFactory /*, Controller ctrlr */, ArmyDispatcher armyDispatcher) {


        this.thisArmy = army;
        this.description = army.ToString() + " army";
        this.nextID = startingID;
        this.warriorFactory = warriorFactory;
        this.buildingFactory = buildingFactory;
//        this.controller = ctrlr;
        this.dispatcher = armyDispatcher;

        ///////////
        // Lists of created (or joined) warriors and buildings in the army //
        /////////////////////////////////////////////////////////////////////
        warriors = new Dictionary<int, AbstractGameUnit>();
        buildings = new Dictionary<int, AbstractGameUnit>();

        // State Machine, controlling all army actions //
        this.stateMachine = new ArmyStateMachine(new ArmyStateData(this));

        ArmySMExecutorComponent executor = GameManager.Instance.gameObject.AddComponent<ArmySMExecutorComponent>();
        executor.SetStateMachine(stateMachine);

    }



}
