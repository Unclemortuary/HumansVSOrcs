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


    private GameResources availableResources;
    public  GameResources AvailableResources {
        get {
            return availableResources;
        }
    }
    public void SetResources(GameResources resources) {
        availableResources = resources;
    }



    private int nextID;


    private Dictionary<int, AbstractGameUnit> warriors;
    private Dictionary<int, AbstractGameUnit> buildings;
    public AbstractGameUnitsList Warriors {
        get {
            return Dict2Values(warriors);
        }
    }
    public AbstractGameUnitsList Buildings {
        get {
            return Dict2Values(buildings);
        }
    }
    private AbstractGameUnitsList Dict2Values(Dictionary<int, AbstractGameUnit> dict) {
        AbstractGameUnitsList lst = new AbstractGameUnitsList();

        foreach (int i in dict.Keys) {
            lst.Add(dict[i]);
        }

        return lst;
    }




    private Dictionary<int, Identification.UnitType> id2typeDictionary;


    /////////////////////////////////////////////////////////////////////
    public bool AreEverybodyDead() {
//        Debug.Log("warcount = " + warriors.Count + ", buildCount = " + buildings.Count);
        return (warriors.Count == 0 && buildings.Count == 0);
    }
    /////////////////////////////////////////////////////////////////////

    private string GenerateDescription(Identification.UnitType type, AbstractGameUnit unit) {
        StringBuilder sb = new StringBuilder();

        sb.Append(type.ToString());
        sb.Append(" of " + thisArmy.ToString());

//        sb.Append("Unit of type ").Append(type.ToString());
//        sb.Append(" in army of " + thisArmy.ToString());
//
        return sb.ToString();
    }

    public GameObject CreateBuildingGhost(Identification.UnitType type, Vector3 position) {
        GameObject go = buildingFactory.CreateGameUnit(type, position).Avatar;
        go.AddComponent<BuildingComponent>();
        return go;
    }

    public void AddRTSActionToUnit(Identification.UnitType type, RTSActionType actionType) {
        CommonGameUnitFactory targetFactory = null;
        if (buildingFactory.CanCreateByType(type)) {
            targetFactory = buildingFactory;
        } else if (warriorFactory.CanCreateByType(type)) {
            targetFactory = warriorFactory;
        }

        if(targetFactory != null) {
            GameUnitCharacteristics characteristics = targetFactory.GetGameUnitCharacteristics(type);
            characteristics.ActionsList.Add(actionType);
        }
    }

    public void RemoveRTSActionForEveryUnit(RTSActionType actionType) {


        foreach(int id in warriors.Keys) {
            RemoveRTSActionForUnit(warriors[id], actionType);
        }

        foreach(int id in buildings.Keys) {
            RemoveRTSActionForUnit(buildings[id], actionType);
        }

        GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
    }

    private void RemoveRTSActionForUnit(AbstractGameUnit unit, RTSActionType actionType) {
        Debug.Log("unit " + unit.Description + " -- checking for action " + actionType.ToString());
        if (unit.Characteristics.ActionsList.Contains(actionType)) {
            unit.Characteristics.ActionsList.Remove(actionType);
        }
    }


    private AbstractGameUnit CreateUnit(CommonGameUnitFactory factory, Identification.UnitType type,
            Vector3 position, Dictionary<int, AbstractGameUnit> unitsStorage) {

        Debug.Log("ArmyManager:: Creating unit of type " + type.ToString() + " in army " + thisArmy.ToString());

        AbstractGameUnit newUnit = factory.CreateGameUnit(type, position);

        newUnit.Description = GenerateDescription(type, newUnit);

        AssimilateUnit(newUnit, unitsStorage);

        id2typeDictionary.Add(newUnit.ID, type);

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

        UnitStateMachine unitStateMachine = newUnit.Avatar.AddComponent<UnitStateMachine>();
        NavMeshAgent agent = newUnit.Avatar.AddComponent<NavMeshAgent>();
//        agent.autoBraking = true;


        unitStateMachine.SetUnitStateMachineHelper( new UnitStateMachineHelper(newUnit, agent, this) );


//        UnitReactionsComponent reactionsComponent = newUnit.Avatar.AddComponent<UnitReactionsComponent>();
//        reactionsComponent.SetGameUnit(newUnit);
//
//        // Add navmesh //
//        NavMeshAgent agent = newUnit.Avatar.AddComponent<NavMeshAgent>();
//        reactionsComponent.SetNavmeshAgent(agent);

        // Set navmeshAgent size the same as collider //
//        Debug.Log("Collider extents = " + newUnit.Avatar.GetComponent<Collider>().bounds.extents);
//        Debug.Log("xxx=" + newUnit.Avatar.GetComponent<Collider>().bounds.extents / newUnit.Avatar.transform.localScale.x);


        // Shamanizm //
        Vector3 boundsExtents = newUnit.Avatar.GetComponent<Collider>().bounds.extents;
        agent.radius = boundsExtents.x / newUnit.Avatar.transform.localScale.x;
        agent.height = boundsExtents.y / newUnit.Avatar.transform.localScale.y;


        agent.speed = newUnit.Characteristics.MaxMovingSpeed;

//        //////////////////////////////////
//        AvailableResources.ChangeResourceAmount(GameResources.ResourceType.MEN, 1);
//        //////////////////////////////////

        return newUnit;
    }

    public AbstractGameUnit CreateWorker(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = CreateWarrior(type, position);

//        UnitReactionsComponent reactionsComponent = newUnit.Avatar.GetComponent<UnitReactionsComponent>();
//
//        BuilderReactionsComponent builderReactions = newUnit.Avatar.AddComponent<BuilderReactionsComponent>();
//        builderReactions.Initialize(reactionsComponent);
//

        return newUnit;
    }

    public AbstractGameUnit CreateFlyingWarrior(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = CreateUnit(warriorFactory, type, position, warriors);

        Rigidbody rb = newUnit.Avatar.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;

        FlyingUnitReactionsComponent reactionsComponent = newUnit.Avatar.AddComponent<FlyingUnitReactionsComponent>();
        reactionsComponent.SetGameUnit(newUnit);

//        //////////////////////////////////
//        AvailableResources.ChangeResourceAmount(GameResources.ResourceType.MEN, 1);
//        //////////////////////////////////

        return newUnit;
    }


    public AbstractGameUnit CreateBuilding(Identification.UnitType type, Vector3 position) {

        AbstractGameUnit newUnit = CreateUnit(buildingFactory, type, position, buildings);

//        UnitReactionsComponent reactionsComponent = newUnit.Avatar.AddComponent<UnitReactionsComponent>();
//        reactionsComponent.SetGameUnit(newUnit);

        UnitStateMachine unitStateMachine = newUnit.Avatar.AddComponent<UnitStateMachine>();

        unitStateMachine.SetUnitStateMachineHelper( new UnitStateMachineHelper(newUnit, this) );


//        NavMeshObstacle obstacle = newUnit.Avatar.AddComponent<NavMeshObstacle>();
//        obstacle.carving = true;
//        reactionsComponent.SetNavmeshAgent(agent);

//        ////////////////////////////////////
//        if (type == Identification.UnitType.GeneralHouse) {
//            AvailableResources.ChangeResourceAmount(GameResources.ResourceType.GENERAL_HOUSES, 1);
//
//        } else if (type == Identification.UnitType.SimpleHouse) {
//            AvailableResources.ChangeResourceAmount(GameResources.ResourceType.LIVING_HOUSES, 1);
//        }
//        ///////////////////////////////////



        // This is a building //
        BuildingComponent buildingComponent = newUnit.Avatar.AddComponent<BuildingComponent>();

        buildingComponent.ChangeGridMapStatus(newUnit.Avatar.transform.position);

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

            Debug.Log("ArmyManager:: Destroying warrior");

            AvailableResources.ChangeResourceAmount(GameResources.ResourceType.MEN, -1);

            warriors.Remove(id);
        }
        if (buildings.ContainsKey(id)) {
            unit =  buildings[id];

            Debug.Log("ArmyManager:: Destroying building");

            if (GetTypeByAbstractGameUnit(unit) == Identification.UnitType.GeneralHouse) {
                AvailableResources.ChangeResourceAmount(GameResources.ResourceType.GENERAL_HOUSES, -1);
            } else if (GetTypeByAbstractGameUnit(unit) == Identification.UnitType.SimpleHouse) {
                AvailableResources.ChangeResourceAmount(GameResources.ResourceType.LIVING_HOUSES, -1);
            }

            buildings.Remove(id);
        }

        if (unit != null) {

            InstantiateDeadBody(unit);

            unit.Nullify();
            GameManager.Instance.PlayerController.PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
        }
    }

    // static ??
    private void InstantiateDeadBody(AbstractGameUnit unit) {
        GameObject deadPrefab = unit.Characteristics.DeadAvatarPrefab;

        GameObject.Instantiate(deadPrefab, unit.Avatar.transform.position, unit.Avatar.transform.rotation);
    }



    public Identification.UnitType GetTypeByAbstractGameUnit(AbstractGameUnit unit) {
        return id2typeDictionary[unit.ID];
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

        this.id2typeDictionary = new Dictionary<int, Identification.UnitType>();


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


        ////////////////// Subscribe ////////////////////////////////

        this.dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("ArmyManager:: Unit is dead: " + unit.Description);

                    DestroyGameUnit(unit.ID);
                }
        );
    }



}
