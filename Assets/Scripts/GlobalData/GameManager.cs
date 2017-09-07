using System;
using System.Collections.Generic;
using Project.TimeManager;
using UnityEditor;
using UnityEngine;

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

    private Identification.Army enemyArmy;


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
    private StartingGamePositions scriptableStartingPositions;




//////////////////////////////////////////////////////////////////////////////////////


    private int orcArmyStartingID = 100000000;
    private int humanArmyStartingID = 20000000;
    private int neutralArmyStartingID = 300000000;

	[SerializeField]
	private Transform _sun;


    [SerializeField]
    private CameraMoverV2 cameraMover;
    [SerializeField]
    private HUDManager hudManager;


    [SerializeField]
    private List<TerrainReactionsComponent> terrainReactionsList = new List<TerrainReactionsComponent>();



    void Start() {
//        StartGameWithPlayerArmyThe(Identification.Army.Orcs);
//        StartGameWithPlayerArmyThe(Identification.Army.Humans);
    }


    public void StartGameWithPlayerArmyThe(Identification.Army armyId) {

// Wait Fro Everyone //
//        while(terrainReactions == null || (cameraHolder == null || hudManager == null)) {
//            Debug.Log("Waiting for terrain, camera, hud");
//        }



        Debug.Log("Start playing as " + armyId.ToString());

        playerArmy = armyId;

        InitializeGame();

        LoadStartingGameUnits();

        InitializeControllers();

        InitializeDependingObjects();

        gameIsStarted = true;


    }


    private void InitializeGame() {

        if (playerArmy == Identification.Army.Humans) {
            enemyArmy = Identification.Army.Orcs;
        } else {
            enemyArmy = Identification.Army.Humans;
        }


        InitializeArmyDispatchers();

        InitializeArmyManagers();

        InitializeActionsLibrary();

        gameObject.AddComponent<TimeManager> ();
        TimeManager.GetInstance.Init (_sun);
    }



    private void InitializeDependingObjects() {


        foreach (TerrainReactionsComponent trc in terrainReactionsList) {
            trc.SetDispatcher(armyDispatchers[playerArmy]);//playerController.PlayerArmyDispatcher);
            trc.InitializeEventTrigger();
        }

//            if (terrainReactions != null) {
//                terrainReactions.SetDispatcher(playerController.PlayerArmyDispatcher);
//            }
//        terrainReactions.InitializeEventTrigger();


//        cameraHolder

        hudManager.InitHUD();

    }


//    private void Start() {
//        LoadStartingGameUnits();
//    }

    private void LoadStartingGameUnits() {
        List<StartingGamePositions.GameUnitSpawnPoint> units = scriptableStartingPositions.GetUnitsPositions();
        List<StartingGamePositions.GameUnitSpawnPoint> buildings = scriptableStartingPositions.GetBuildingsPositions();


        foreach(StartingGamePositions.ArmyResourcesPair resPair in scriptableStartingPositions.StartingResources) {
            armyManagers[resPair.ArmyType].SetResources(resPair.ResourcesAmount.CreateCopy());
        }


        foreach (StartingGamePositions.GameUnitSpawnPoint item in units) {
            armyManagers[item.ArmyType].CreateWarrior(item.UnitType, item.Position);
        }
        foreach (StartingGamePositions.GameUnitSpawnPoint item in buildings) {
            AbstractGameUnit newBuilding = armyManagers[item.ArmyType].CreateBuilding(item.UnitType, item.Position);

//            BuildingComponent buildingComponent = newBuilding.Avatar.GetComponent<BuildingComponent>();
//            buildingComponent.ChangeGridMapStatus(newBuilding.Avatar.transform.position);
        }

        foreach(StartingGamePositions.CameraPosition cameraPosition in scriptableStartingPositions.CameraHolderPositions) {
            Debug.Log("Checking camera position for " + cameraPosition.Army.ToString());
            if (cameraPosition.Army == playerArmy) {
                Debug.Log("Set starting position for camera");
                cameraMover.SetStartingPosition(cameraPosition.Position);
//                cameraHolder.gameObject.transform.position = cameraPosition.Position;
            }
        }
    }

    private void InitializeArmyDispatchers() {
        armyDispatchers = new Dictionary<Identification.Army, ArmyDispatcher>();

        foreach (Identification.Army armyType in Enum.GetValues(typeof(Identification.Army))) {
            armyDispatchers.Add(armyType, new ArmyDispatcher());
        }

    }


    private void InitializeActionsLibrary() {

        actionsLibrary = new RTSActionLibrary();

        /// Add here all possible RTSActions //
        /// And initialize their information for Panel of Commands //
        ///
        ///
//        ActionData.SomeAction dummyData = new ActionData.SomeAction("", RTSActionType.NULL, null);

        actionsLibrary.AddRTSAction(RTSActionType.moveTo, new MoveToAction());
        actionsLibrary.AddRTSAction(RTSActionType.moveFormationTo5, new MoveFormationToAction(5));
        actionsLibrary.AddRTSAction(RTSActionType.moveFormationTo10, new MoveFormationToAction(10));
        actionsLibrary.AddRTSAction(RTSActionType.moveFormationTo15, new MoveFormationToAction(15));

        actionsLibrary.AddRTSAction(RTSActionType.stop, new StopAction());

        actionsLibrary.AddRTSAction(RTSActionType.attack, new AttackAction(5));

        actionsLibrary.AddRTSAction(RTSActionType.createArcher, new CreateUnitAction(Identification.UnitType.Archer));
        actionsLibrary.AddRTSAction(RTSActionType.createSwordsman, new CreateUnitAction(Identification.UnitType.Swordsman));
        actionsLibrary.AddRTSAction(RTSActionType.createHorseman, new CreateUnitAction(Identification.UnitType.Horseman));
        actionsLibrary.AddRTSAction(RTSActionType.createWorker, new CreateUnitAction(Identification.UnitType.Worker));
        actionsLibrary.AddRTSAction(RTSActionType.createFlyingWarrior, new CreateUnitAction(Identification.UnitType.FlyingWarrior));


//        actionsLibrary.AddRTSAction(RTSActionType.buildFarm, new BuildAction(Identification.UnitType.Farm));
//        actionsLibrary.AddRTSAction(RTSActionType.buildGeneralHouse, new BuildAction(Identification.UnitType.GeneralHouse));
//        actionsLibrary.AddRTSAction(RTSActionType.buildBarrack, new BuildAction(Identification.UnitType.Barrack));
//        actionsLibrary.AddRTSAction(RTSActionType.buildForge, new BuildAction(Identification.UnitType.Forge));
//        actionsLibrary.AddRTSAction(RTSActionType.buildQuarry, new BuildAction(Identification.UnitType.Quarry));
//        actionsLibrary.AddRTSAction(RTSActionType.buildSawmill, new BuildAction(Identification.UnitType.Sawmill));
//        actionsLibrary.AddRTSAction(RTSActionType.buildSimpleHouse, new BuildAction(Identification.UnitType.SimpleHouse));
//        actionsLibrary.AddRTSAction(RTSActionType.buildWatchTower, new BuildAction(Identification.UnitType.WatchTower));

        actionsLibrary.AddRTSAction(RTSActionType.workersBuildFarm, new WorkerBuildAction(Identification.UnitType.Farm));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildGeneralHouse, new WorkerBuildAction(Identification.UnitType.GeneralHouse));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildBarrack, new WorkerBuildAction(Identification.UnitType.Barrack));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildForge, new WorkerBuildAction(Identification.UnitType.Forge));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildQuarry, new WorkerBuildAction(Identification.UnitType.Quarry));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildSawmill, new WorkerBuildAction(Identification.UnitType.Sawmill));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildSimpleHouse, new WorkerBuildAction(Identification.UnitType.SimpleHouse));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildWatchTower, new WorkerBuildAction(Identification.UnitType.WatchTower));
        actionsLibrary.AddRTSAction(RTSActionType.workersBuildMine, new WorkerBuildAction(Identification.UnitType.Mine));



        actionsLibrary.AddRTSAction(RTSActionType.createSwordsman2, new CreateUnitAction(Identification.UnitType.Swordsman2));
        actionsLibrary.AddRTSAction(RTSActionType.createSwordsman3, new CreateUnitAction(Identification.UnitType.Swordsman3));
        actionsLibrary.AddRTSAction(RTSActionType.createArcher2, new CreateUnitAction(Identification.UnitType.Archer2));
        actionsLibrary.AddRTSAction(RTSActionType.createArcher3, new CreateUnitAction(Identification.UnitType.Archer3));



        Dictionary<Identification.UnitType, RTSActionType> unitActionDictionary = null;

        unitActionDictionary = new Dictionary<Identification.UnitType, RTSActionType>();
        unitActionDictionary.Add(Identification.UnitType.Barrack, RTSActionType.createArcher2);
        unitActionDictionary.Add(Identification.UnitType.Forge, RTSActionType.upgradeArcherTo3);
        actionsLibrary.AddRTSAction(RTSActionType.upgradeArcherTo2,
                new AddSomeNewRTSActionsToUnitsAction(RTSActionType.upgradeArcherTo2, unitActionDictionary)
        );

        unitActionDictionary = new Dictionary<Identification.UnitType, RTSActionType>();
        unitActionDictionary.Add(Identification.UnitType.Barrack, RTSActionType.createArcher3);
        actionsLibrary.AddRTSAction(RTSActionType.upgradeArcherTo3,
                new AddSomeNewRTSActionsToUnitsAction(RTSActionType.upgradeArcherTo3, unitActionDictionary)
        );


        unitActionDictionary = new Dictionary<Identification.UnitType, RTSActionType>();
        unitActionDictionary.Add(Identification.UnitType.Barrack, RTSActionType.createSwordsman2);
        unitActionDictionary.Add(Identification.UnitType.Forge, RTSActionType.upgradeSwordsmanTo3);
        actionsLibrary.AddRTSAction(RTSActionType.upgradeSwordsmanTo2,
                new AddSomeNewRTSActionsToUnitsAction(RTSActionType.upgradeSwordsmanTo2, unitActionDictionary)
        );

        unitActionDictionary = new Dictionary<Identification.UnitType, RTSActionType>();
        unitActionDictionary.Add(Identification.UnitType.Barrack, RTSActionType.createSwordsman3);
        actionsLibrary.AddRTSAction(RTSActionType.upgradeSwordsmanTo3,
                new AddSomeNewRTSActionsToUnitsAction(RTSActionType.upgradeSwordsmanTo3, unitActionDictionary)
        );




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
        InitAFactory(neutralWarriorFactory, scriptablePrototypesList.NeutralWarriorsPrototypes);

        CommonGameUnitFactory neutralBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(neutralBuildingFactory, scriptablePrototypesList.NeutralBuildingsPrototypes);

        armyManagers[Identification.Army.Neutrals] = new ArmyManager(Identification.Army.Neutrals,
                neutralArmyStartingID, neutralWarriorFactory, neutralBuildingFactory,
                armyDispatchers[Identification.Army.Neutrals]);



    } // initialize army managers //


    // Controllers ///////////////////////////////////////////////////////////

    public bool AILightsOn  = false;

    AbstractContorller neutralsController;
    AbstractContorller enemyController;

    [SerializeField]
    private AIScript scriptForAI;

    private void InitializeControllers() {

        playerController = new PlayerController(armyManagers[playerArmy]);

        // Neutrals controller //

        neutralsController = new NeutralsController(armyManagers[Identification.Army.Neutrals]);

        // AI controllers //

        AILightsOn = scriptForAI.AiLightsOn;

        if(playerArmy == Identification.Army.Orcs) {
            enemyController = new AIController(armyManagers[Identification.Army.Humans], scriptForAI, gameObject);
//            enemyController = new NeutralsController(armyManagers[Identification.Army.Humans]);
        } else {
            enemyController = new AIController(armyManagers[Identification.Army.Orcs], scriptForAI, gameObject);
//            enemyController = new NeutralsController(armyManagers[Identification.Army.Orcs]);
        }


    } // InitializeControllers() //



//////////////////////////////////////////////////////////////////////////////////////////////
// ######################################################################################## //
//////////////////////////////////////////////////////////////////////////////////////////////



    public void HereIAm(RTSMonoBehaviour monobeh) {
        GameUnitID uid = monobeh.gameObject.GetComponent<GameUnitID>();

        if (uid != null) {
            monobeh.SetArmyManager(armyManagers[uid.Army]);
//            monobeh.SetArmyDispatcher(armyDispatchers[uid.Army]);
        }

    }


    public void HereIsTerrain(TerrainReactionsComponent reactionsScript) {



        this.terrainReactionsList.Add(reactionsScript);

//        if (terrainReactions != null) {
//            terrainReactions.SetDispatcher(playerController.PlayerArmyDispatcher);
//        }
//        terrainReactions.InitializeEventTrigger();

        Debug.Log("-> Terrain is added to GameManager");

    }

    public void HereIsCameraHolder(CameraHolderComponent holder) {


//        this.cameraMover = holder;
//
//        Debug.Log("-> CameraHoder is initialized");
    }


    public void HereIsHud(HUDManager hmanager) {
//        this.hudManager = hmanager;
//
//        Debug.Log("-> HudManager is initialized");
    }

    /// GLOBAL SAVE GAMEUNITS POSITIONS ///////////////////////////////////////////////
    ///

    private class SpawnPointsList : List<StartingGamePositions.GameUnitSpawnPoint> {}

    private class ResourcePairsList : List<StartingGamePositions.ArmyResourcesPair> {}

    private class CameraPositionsList : List<StartingGamePositions.CameraPosition> {}

    public void SaveCurrentGamePositions() {


        StartingGamePositions positions =  ScriptableObject.CreateInstance<StartingGamePositions>();

        SpawnPointsList unitsPoints = new SpawnPointsList();
        SpawnPointsList buildingsPoints = new SpawnPointsList();
        ResourcePairsList resourcePairsList = new ResourcePairsList();
        CameraPositionsList cameraPositions = new CameraPositionsList();

        foreach (Identification.Army armyType in Enum.GetValues(typeof(Identification.Army))) {
            ArmyManager manager = Instance.ArmyManagers[armyType];
            if (manager != null) {

                AbstractGameUnitsList warriorsList = manager.Warriors;
                AddRespawnPointsToPointsList(unitsPoints, warriorsList, manager);

                AbstractGameUnitsList buildingsList = manager.Buildings;
                AddRespawnPointsToPointsList(buildingsPoints, buildingsList, manager);

                StartingGamePositions.ArmyResourcesPair pair =
                    new StartingGamePositions.ArmyResourcesPair(armyType, manager.AvailableResources.CreateCopy());
                resourcePairsList.Add(pair);

//                StartingGamePositions.CameraPosition camPos = scriptableStartingPositions.CameraHolderPositions;
//                cameraPositions.Add(new StartingGamePositions.CameraPosition(camPos.Army, camPos.Position));

            } // If we have such manager //

        } // foreach armyType //

        positions.SetBuildingsPositions(buildingsPoints);
        positions.SetUnitsPositions(unitsPoints);
        positions.StartingResources = resourcePairsList;
        positions.CameraHolderPositions = scriptableStartingPositions.CameraHolderPositions;
/////////////////////////////////////////////////////////////////////////



        DateTime now = DateTime.Now;
        string nowString = "" + now.Year + "_" + now.Month + "_" + now.Day + "__" + now.Hour + "_" + now.Minute;
        Debug.Log(nowString);


        AssetDatabase.CreateAsset(positions, "Assets/ScriptableObjectsData/Saves/" + nowString + ".asset");
        AssetDatabase.SaveAssets();

    }


    private void AddRespawnPointsToPointsList(SpawnPointsList pointsList, AbstractGameUnitsList unitsList, ArmyManager manager) {

        foreach(AbstractGameUnit unit in unitsList) {

            Vector3 unitPosition = unit.Avatar.transform.position;

            pointsList.Add(new StartingGamePositions.GameUnitSpawnPoint(
                    manager.GetTypeByAbstractGameUnit(unit),
                    manager.ThisArmy,
                    new Vector3(unitPosition.x, unitPosition.y, unitPosition.z)
            ));
        } // Foreach unit in AbstractGameUnitList //

    }




    // PAUSE /////////////////////////////////////////////////////////////

    public void SetGlobalPause(bool condition) {
        if (condition) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public bool IsPaused() {
        return Time.timeScale == 0;
    }

    ///  End Game Checking //////////////////////////////////////////////////////////////////////

    private bool gameIsStarted = false;
    public bool GameIsStarted {
        get {
            return gameIsStarted;
        }
    }


    void Update() {
        CheckEndGame();
    }



    public void CheckEndGame() {

//        foreach (Identification.Army armyType in Enum.GetValues(typeof(Identification.Army))) {
//            armyManagers[armyType]...
//        }


        if (gameIsStarted) {
            if (armyManagers[playerArmy].AreEverybodyDead()) {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!! Computer won !!!!!!!!!!!!!!!!!!!");

                gameObject.GetComponent<MenuEndGame>().PlayerWon(false);

                gameIsStarted = false;
//                SetGlobalPause(true);
            } else if (armyManagers[enemyArmy].AreEverybodyDead()){
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!! Player won !!!!!!!!!!!!!!!!!!!");

                gameObject.GetComponent<MenuEndGame>().PlayerWon(true);

                gameIsStarted = false;
//                SetGlobalPause(true);
            }
        }

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////



} // End of class //

