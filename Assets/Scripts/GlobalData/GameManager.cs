using System;
using System.Collections.Generic;
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

    private PlayerController playerController;
    public PlayerController PlayerController {
        get {
            return playerController;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    [SerializeField]
    private ScriptablePrototypesDictionaries scriptablePrototypesList;


    //////////////////////////////////////////////////////////////////////////////////////


    private int orcArmyStartingID = 100000000;
    private int humanArmyStartingID = 20000000;
    private int goblinArmyStartingID = 300000000;




    private void InitializeGame() {

        InitializeArmyDispatchers();

        InitializeArmyManagers();

        InitializeActionsLibrary();

        InitializeControllers();

    }

    private void InitializeActionsLibrary() {

        actionsLibrary = new RTSActionLibrary();

        /// Add here all possible RTSActions //
        /// And initialize their CommandPanelItems //
        ///

        actionsLibrary.AddRTSAction(RTSActionType.moveTo, new MoveToAction(
            new CommandPanelItem("MoveTo")
        ));

// ################################################################################################################################
// ################################################################################################################################
// ################################################################################################################################
//        move, stop, attack, build,
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



    } // initialize army managers //



    private void InitializeControllers() {

        playerController = new PlayerController(armyManagers[Identification.Army.Humans]);

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

