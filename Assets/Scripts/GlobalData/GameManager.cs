using System;
using System.Collections.Generic;
using UnityEngine;

using Project.TimeManager;

public class GameManager : MonoBehaviour {



// * Singleton * //

    private static GameManager _instance = null;
    public static GameManager Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            if (_instance != this) {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        ////////////////////////////
        InitializeGame();
    }

// ********************************************************************************** //
// ********************************************************************************** //

    private Dictionary<Identification.Army, ArmyManager> armyManagers;

    public Dictionary<Identification.Army, ArmyManager> ArmyManagers {
        get {
            return armyManagers;
        }
    }


    private Dictionary<Identification.Army, ArmyDispatcher> armyDispatchers;


    private RTSActionLibrary actionsLibrary;
    public RTSActionLibrary ActionsLibrary {
        get {
            return actionsLibrary;
        }
    }

    private Identification.Army playerArmy = Identification.Army.Humans;
    public Identification.Army PlayerArmy {
        get {
            return playerArmy;
        }
    }

    private PlayerController playerController;
    public PlayerController PlayerController {
        get {
            return playerController;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    // Fields to be defined via Inspector ///////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    [SerializeField]
    private ScriptablePrototypesDictionaries scriptablePrototypesList;

    [SerializeField]
    private ActionData scriptableActionDataList;

    [SerializeField]
    private StartingGameUnitsPositions scriptableStartingPositions;




//////////////////////////////////////////////////////////////////////////////////////


    private int orcArmyStartingID = 100000000;
    private int humanArmyStartingID = 20000000;
    private int neutralArmyStartingID = 300000000;

	[SerializeField]
	private Transform _sun;



    private void Start() {
        LoadStartingGameUnits();
    }

    private void LoadStartingGameUnits() {
        List<StartingGameUnitsPositions.GameUnitSpawnPoint> units = scriptableStartingPositions.GetUnitsPositions();
        List<StartingGameUnitsPositions.GameUnitSpawnPoint> buildings = scriptableStartingPositions.GetBuildingsPositions();

        foreach (StartingGameUnitsPositions.GameUnitSpawnPoint item in units) {
            armyManagers[item.ArmyType].CreateWarrior(item.UnitType, item.Position);
        }
        foreach (StartingGameUnitsPositions.GameUnitSpawnPoint item in buildings) {
            armyManagers[item.ArmyType].CreateBuilding(item.UnitType, item.Position);
        }

        foreach(StartingGameUnitsPositions.ArmyResourcesPair resPair in scriptableStartingPositions.StartingResources) {
            armyManagers[resPair.ArmyType].SetResources(resPair.ResourcesAmount.CreateCopy());
        }
    }

    private void InitializeGame() {

        InitializeArmyDispatchers();

        InitializeArmyManagers();

        InitializeActionsLibrary();

        InitializeControllers();

		gameObject.AddComponent<TimeManager> ();
		TimeManager.GetInstance.Init (_sun);
    }

    private void InitializeActionsLibrary() {

        actionsLibrary = new RTSActionLibrary();

        /// Add here all possible RTSActions //
        /// And initialize their information for Panel of Commands //
        ///
        ///
//        ActionData.SomeAction dummyData = new ActionData.SomeAction("", RTSActionType.NULL, null);

        actionsLibrary.AddRTSAction(RTSActionType.moveTo, new MoveToAction());

        actionsLibrary.AddRTSAction(RTSActionType.stop, new StopAction());

        actionsLibrary.AddRTSAction(RTSActionType.createArcher, new CreateUnitAction(Identification.UnitType.Archer));
        actionsLibrary.AddRTSAction(RTSActionType.createSwordsman, new CreateUnitAction(Identification.UnitType.Swordsman));
        actionsLibrary.AddRTSAction(RTSActionType.createHorseman, new CreateUnitAction(Identification.UnitType.Horseman));
        actionsLibrary.AddRTSAction(RTSActionType.createWorker, new CreateUnitAction(Identification.UnitType.Worker));


        actionsLibrary.AddRTSAction(RTSActionType.buildFarm, new BuildAction(Identification.UnitType.Farm));
        actionsLibrary.AddRTSAction(RTSActionType.buildGeneralHouse, new BuildAction(Identification.UnitType.GeneralHouse));
        actionsLibrary.AddRTSAction(RTSActionType.buildBarrack, new BuildAction(Identification.UnitType.Barrack));
        actionsLibrary.AddRTSAction(RTSActionType.buildForge, new BuildAction(Identification.UnitType.Forge));
        actionsLibrary.AddRTSAction(RTSActionType.buildQuarry, new BuildAction(Identification.UnitType.Quarry));
        actionsLibrary.AddRTSAction(RTSActionType.buildSawmill, new BuildAction(Identification.UnitType.Sawmill));
        actionsLibrary.AddRTSAction(RTSActionType.buildSimpleHouse, new BuildAction(Identification.UnitType.SimpleHouse));
        actionsLibrary.AddRTSAction(RTSActionType.buildWatchTower, new BuildAction(Identification.UnitType.WatchTower));

        actionsLibrary.AddRTSAction(RTSActionType.workersBuildFarm, new WorkerBuildAction(Identification.UnitType.Farm));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildGeneralHouse, new WorkerBuildAction(Identification.UnitType.GeneralHouse));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildBarrack, new WorkerBuildAction(Identification.UnitType.Barrack));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildForge, new WorkerBuildAction(Identification.UnitType.Forge));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildQuarry, new WorkerBuildAction(Identification.UnitType.Quarry));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildSawmill, new WorkerBuildAction(Identification.UnitType.Sawmill));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildSimpleHouse, new WorkerBuildAction(Identification.UnitType.SimpleHouse));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildWatchTower, new WorkerBuildAction(Identification.UnitType.WatchTower));

//        Debug.Log ("GameManager: Reading ActionData");
        // Initialize ActionData form scriptableObject //
        foreach (ActionData.ActionDataItem actionDataItem in scriptableActionDataList.ActionsData) {
            if (actionsLibrary.Contains(actionDataItem.Action)) {
//                Debug.Log("GM: action=" + actionDataItem.Action.ToString());
                actionsLibrary.GetRTSAction(actionDataItem.Action).SetActionDataItem(actionDataItem);
            }
        }

//        Debug.Log ("GameManager: Finish reading ActionData");


// ################################################################################################################################
// ################################################################################################################################
// ################################################################################################################################
//        attack,
//        holdPosition, patrol,
//        specialHeal, specialFireStorm

    }


    private void InitializeArmyDispatchers() {
        armyDispatchers = new Dictionary<Identification.Army, ArmyDispatcher>();

        foreach (Identification.Army armyType in Enum.GetValues(typeof(Identification.Army))) {
            armyDispatchers.Add(armyType, new ArmyDispatcher());
        }

    }


    private void InitAFactory(CommonGameUnitFactory factory, TypeToGameUnitDictionary prototypes)
    {
        foreach (Identification.UnitType unitType in Enum.GetValues(typeof(Identification.UnitType))) {

            if (prototypes.ContainsKey(unitType)) {

//                Debug.Log("init factory type=" + unitType.ToString());

                ClonnableGameUnit unitPrototype = prototypes[unitType];

                if (unitPrototype != null) {
                    factory.AddCreator(
                            unitType,
                            new CommonGameUnitCloneCreator(unitPrototype.CreateCopy())
                    );
                }

            } // if contains key //

        } // foreach type //

    } // InitAFactory() //


    private void InitializeArmyManagers() {

        armyManagers = new Dictionary<Identification.Army, ArmyManager>();

        // Human Army Manager //

        CommonGameUnitFactory humanWarriorFactory = new CommonGameUnitFactory();
        InitAFactory(humanWarriorFactory, scriptablePrototypesList.HumanWarriorsPrototypes);

        CommonGameUnitFactory humanBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(humanBuildingFactory, scriptablePrototypesList.HumanBuildingsPrototypes);

        armyManagers[Identification.Army.Humans] = new ArmyManager(Identification.Army.Humans,
                humanArmyStartingID, humanWarriorFactory, humanBuildingFactory,
                armyDispatchers[Identification.Army.Humans]);


        // Orc Army Manager //

        CommonGameUnitFactory orcWarriorFactory = new CommonGameUnitFactory();
        InitAFactory(orcWarriorFactory, scriptablePrototypesList.OrcWarriorsPrototypes);

        CommonGameUnitFactory orcBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(orcBuildingFactory, scriptablePrototypesList.OrcBuildingsPrototypes);

        armyManagers[Identification.Army.Orcs] = new ArmyManager(Identification.Army.Orcs,
                orcArmyStartingID, orcWarriorFactory, orcBuildingFactory,
                armyDispatchers[Identification.Army.Orcs]);


        // Neutrals Army Manager //

        CommonGameUnitFactory neutralWarriorFactory = new CommonGameUnitFactory();
        InitAFactory(neutralWarriorFactory, scriptablePrototypesList.OrcWarriorsPrototypes);

        CommonGameUnitFactory neutralBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(neutralBuildingFactory, scriptablePrototypesList.OrcBuildingsPrototypes);

        armyManagers[Identification.Army.Neutrals] = new ArmyManager(Identification.Army.Neutrals,
                neutralArmyStartingID, neutralWarriorFactory, neutralBuildingFactory,
                armyDispatchers[Identification.Army.Neutrals]);



    } // initialize army managers //



    private void InitializeControllers() {

        playerController = new PlayerController(armyManagers[playerArmy]);

        // AI controllers //


    }


/////////////////////////////////

    public void HereIAm(RTSMonoBehaviour monobeh) {
        GameUnitID uid = monobeh.gameObject.GetComponent<GameUnitID>();

        if (uid != null) {
            monobeh.SetArmyManager(armyManagers[uid.Army]);
//            monobeh.SetArmyDispatcher(armyDispatchers[uid.Army]);
        }

    }


    public void HereIsTerrain(TerrainReactionsComponent reactionsScript) {

        if (reactionsScript != null) {
            reactionsScript.SetDispatcher(playerController.PlayerArmyDispatcher);
        }
    }



} // End of class //

