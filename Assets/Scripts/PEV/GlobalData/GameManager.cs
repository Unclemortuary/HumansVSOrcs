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
    private ScriptablePrototypesLists scriptablePrototypesList;
    private int archerIndex = 0; //(int)Identification.UnitType.Archer;
    private int swordsmanIndex = 1;
    private int horsemanIndex = 2;

    private int palaceIndex = 0;
    private int barrackIndex = 1;


    //////////////////////////////////////////////////////////////////////////////////////


    private int orcArmyStartingID = 100000000;
    private int humanArmyStartingID = 20000000;
    private int goblinArmyStartingID = 300000000;

//    private string orcArmyDescription = "Orcs Army";
//    private string humanArmyDescription = "Humans Army";

    private Dictionary<Identification.UnitType, ClonnableGameUnit> humanWarriorsPrototypes;
    private Dictionary<Identification.UnitType, ClonnableGameUnit> orcWarriorsPrototypes;

    private Dictionary<Identification.UnitType, ClonnableGameUnit> humanBuildingsPrototypes;
    private Dictionary<Identification.UnitType, ClonnableGameUnit> orcBuildingsPrototypes;





    private void InitializeGame() {

        initializePrototypesLists();

        initializeArmyManagers();


    }

    private void initializePrototypesLists() {

        // Warriors //

        humanWarriorsPrototypes = new Dictionary<Identification.UnitType, ClonnableGameUnit>();
        humanWarriorsPrototypes.Add(Identification.UnitType.Archer,
                scriptablePrototypesList.HumanWarriorsPrototypes[archerIndex].CreateCopy());
        humanWarriorsPrototypes.Add(Identification.UnitType.Swordsman,
                scriptablePrototypesList.HumanWarriorsPrototypes[swordsmanIndex].CreateCopy());
        humanWarriorsPrototypes.Add(Identification.UnitType.Horseman,
                scriptablePrototypesList.HumanWarriorsPrototypes[horsemanIndex].CreateCopy());

        orcWarriorsPrototypes = new Dictionary<Identification.UnitType, ClonnableGameUnit>();
        orcWarriorsPrototypes.Add(Identification.UnitType.Archer,
                scriptablePrototypesList.OrcWarriorsPrototypes[archerIndex].CreateCopy());
        orcWarriorsPrototypes.Add(Identification.UnitType.Swordsman,
                scriptablePrototypesList.OrcWarriorsPrototypes[swordsmanIndex].CreateCopy());
        orcWarriorsPrototypes.Add(Identification.UnitType.Horseman,
                scriptablePrototypesList.OrcWarriorsPrototypes[horsemanIndex].CreateCopy());

        // Buildings //

        humanBuildingsPrototypes = new Dictionary<Identification.UnitType, ClonnableGameUnit>();
        humanBuildingsPrototypes.Add(Identification.UnitType.Palace,
                scriptablePrototypesList.HumanBuildingsPrototypes[palaceIndex].CreateCopy());
        humanBuildingsPrototypes.Add(Identification.UnitType.Barrack,
                scriptablePrototypesList.HumanBuildingsPrototypes[barrackIndex].CreateCopy());


        orcBuildingsPrototypes = new Dictionary<Identification.UnitType, ClonnableGameUnit>();
        orcBuildingsPrototypes.Add(Identification.UnitType.Palace,
                scriptablePrototypesList.OrcBuildingsPrototypes[palaceIndex].CreateCopy());
        orcBuildingsPrototypes.Add(Identification.UnitType.Barrack,
                scriptablePrototypesList.OrcBuildingsPrototypes[barrackIndex].CreateCopy());


    } // initialize prototypes //



    private void initializeArmyManagers() {

        CommonGameUnitFactory humanWarriorFactory = new CommonGameUnitFactory();
        humanWarriorFactory.AddCreator(Identification.UnitType.Archer,
                new CommonGameUnitCloneCreator(humanWarriorsPrototypes[Identification.UnitType.Archer])
                );
        humanWarriorFactory.AddCreator(Identification.UnitType.Swordsman,
                new CommonGameUnitCloneCreator(humanWarriorsPrototypes[Identification.UnitType.Swordsman])
                );
        humanWarriorFactory.AddCreator(Identification.UnitType.Horseman,
                new CommonGameUnitCloneCreator(humanWarriorsPrototypes[Identification.UnitType.Horseman])
                );

        CommonGameUnitFactory orcWarriorFactory = new CommonGameUnitFactory();
        orcWarriorFactory.AddCreator(Identification.UnitType.Archer,
                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Archer])
                );
        orcWarriorFactory.AddCreator(Identification.UnitType.Swordsman,
                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Swordsman])
                );
        orcWarriorFactory.AddCreator(Identification.UnitType.Horseman,
                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Horseman])
                );


//        WarriorFactory orcWarriorFactory = new WarriorFactory(
//                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Archer]),
//                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Swordsman]),
//                new CommonGameUnitCloneCreator(orcWarriorsPrototypes[Identification.UnitType.Horseman]));
//        HumanBuildingFactory humanBuildingFactory = new HumanBuildingFactory();
//        OrcBuildingFactory orcBuildingFactory = new OrcBuildingFactory();


        CommonGameUnitFactory humanBuildingFactory = new CommonGameUnitFactory();
        humanBuildingFactory.AddCreator(Identification.UnitType.Palace,
                new CommonGameUnitCloneCreator(humanBuildingsPrototypes[Identification.UnitType.Palace])
        );
        humanBuildingFactory.AddCreator(Identification.UnitType.Barrack,
                new CommonGameUnitCloneCreator(humanBuildingsPrototypes[Identification.UnitType.Barrack])
        );


        CommonGameUnitFactory orcBuildingFactory = new CommonGameUnitFactory();
        orcBuildingFactory.AddCreator(Identification.UnitType.Palace,
                new CommonGameUnitCloneCreator(orcBuildingsPrototypes[Identification.UnitType.Palace])
        );
        orcBuildingFactory.AddCreator(Identification.UnitType.Barrack,
                new CommonGameUnitCloneCreator(orcBuildingsPrototypes[Identification.UnitType.Barrack])
        );


        humanArmyManager = new ArmyManager(Identification.Army.Humans, humanArmyStartingID, humanWarriorFactory, humanBuildingFactory);
        orcArmyManager = new ArmyManager(Identification.Army.Orcs, orcArmyStartingID, orcWarriorFactory, orcBuildingFactory);


        // initialize controllers ******************************************* //
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //

    } // initialize army managers //




} // End of class //

