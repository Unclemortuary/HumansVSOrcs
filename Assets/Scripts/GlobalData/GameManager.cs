using System;
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

    private ArmyManager orcArmyManager;
    public ArmyManager OrcArmyManager {
        get {
            return orcArmyManager;
        }
    }

    private ArmyManager humanArmyManager;
    public ArmyManager HumanArmyManager {
        get {
            return humanArmyManager;
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


        initializeArmyManagers();


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


    private void initializeArmyManagers() {

        // Human Warriors //

        CommonGameUnitFactory humanWarriorFactory = new CommonGameUnitFactory();
        InitAFactory(humanWarriorFactory, scriptablePrototypesList.HumanWarriorsPrototypes);

        CommonGameUnitFactory humanBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(humanBuildingFactory, scriptablePrototypesList.HumanBuildingsPrototypes);

        humanArmyManager = new ArmyManager(Identification.Army.Humans, humanArmyStartingID, humanWarriorFactory, humanBuildingFactory);



        CommonGameUnitFactory orcWarriorFactory = new CommonGameUnitFactory();
        InitAFactory(orcWarriorFactory, scriptablePrototypesList.OrcWarriorsPrototypes);

        CommonGameUnitFactory orcBuildingFactory = new CommonGameUnitFactory();
        InitAFactory(orcBuildingFactory, scriptablePrototypesList.OrcBuildingsPrototypes);

        orcArmyManager = new ArmyManager(Identification.Army.Orcs, orcArmyStartingID, orcWarriorFactory, orcBuildingFactory);


        // initialize controllers ******************************************* //
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //


    } // initialize army managers //



    /////////////////////////////////

    public void HereIAm(RTSMonoBehaviour monobeh) {
//        monobeh.SetArmyManager();
//        monobeh.SetDispatcher();
    }



} // End of class //

