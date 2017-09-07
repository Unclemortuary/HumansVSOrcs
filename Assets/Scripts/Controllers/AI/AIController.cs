using System.Collections.Generic;
using UnityEngine;

public class AIController : AbstractContorller{


//    public class BuildingsDictionary : Dictionary<Identification.UnitType, List<AbstractGameUnit>> {}

    System.Random random = new System.Random();

    private AIScript behaviourScript;

    private enum Commands {
        NULL,

        CreateWorker,
        CreateWarrior,

        BuildGeneral,
        BuildBarrack,
        BuildQuarry,
        BuildMine,
        BuildSawmill,
        BuildForge,
        BuildSimpleHouse,
        BuildWatchtower,
        BuildFarm,

        SendFiveUnitsToAttack,

        RandomUpgrade,

    }


    private Queue<Commands> globalCommandsQueue = new Queue<Commands>();


    public void Update() {
        //
        if (globalCommandsQueue.Count > 0 ) {

            Commands command = globalCommandsQueue.Dequeue();


            Debug.Log("########### >>> AI:: PerformingCommand " + command.ToString());
            PerformCommand(command);

        } else {

            Debug.Log("########### >>> AI:: No Commands in Queue");

            int numberOfWorkers = FindNumberOfWorkers();
            if (numberOfWorkers < behaviourScript.GoodNumberOfWorkers) {
                globalCommandsQueue.Enqueue(Commands.CreateWorker);
            }

            int numberOfWarriors = thisArmyManager.WarriorsCount;

            if (numberOfWarriors >= 10) {
                globalCommandsQueue.Enqueue(Commands.SendFiveUnitsToAttack);
            } else {
                globalCommandsQueue.Enqueue(Commands.CreateWarrior);
                globalCommandsQueue.Enqueue(Commands.CreateWarrior);
            }

//            List<Identification.UnitType> buildingsTypes = behaviourScript.BuildingsPriorityOrder;
//
//            foreach (Identification.UnitType type in buildingsTypes) {
//                int num = FindNumberOfUnitsOfType(type) ;
//
//                if (num < 1) {
//                    Commands buildCommand = GetBuildCommandByType(type);
//                    globalCommandsQueue.Enqueue(buildCommand);
//                    break;
//                }
//            }
//
//
//
//            if ( ! thisArmyManager.AvailableResources.HaveFreeLivingPlaces() ) {
//                globalCommandsQueue.Enqueue(Commands.BuildSimpleHouse);
//            }


            if (FindNumberOfUnitsOfType(Identification.UnitType.Forge) > 0) {
                globalCommandsQueue.Enqueue(Commands.RandomUpgrade);
            }

// update information about number of buildings and units //

// if have enough units, send them to attack one of enemyPoints //

// if have enough resources and do not have enough buildings, Build //
// if worker is unavailable, create worker

// if have enough resources and living places, create warrior //
//
// if barracks unavailable, skip the step //

        }

    }


    private Commands GetBuildCommandByType(Identification.UnitType type) {
        Commands command = Commands.NULL;

        switch (type) {
            case Identification.UnitType.GeneralHouse:
                command = Commands.BuildGeneral;
                break;
            case Identification.UnitType.Barrack:
                command = Commands.BuildBarrack;
                break;
            case Identification.UnitType.Mine:
                command = Commands.BuildMine;
                break;
            case Identification.UnitType.Farm:
                command = Commands.BuildFarm;
                break;
            case Identification.UnitType.Sawmill:
                command = Commands.BuildSawmill;
                break;
            case Identification.UnitType.Quarry:
                command = Commands.BuildQuarry;
                break;
            case Identification.UnitType.Forge:
                command = Commands.BuildForge;
                break;
            case Identification.UnitType.SimpleHouse:
                command = Commands.BuildSimpleHouse;
                break;
            case Identification.UnitType.WatchTower:
                command = Commands.BuildWatchtower;
                break;

        }

        return  command;
    }



    /*

        BuildQuarry,
        BuildMine,
        BuildSawmill,
        BuildForge,
        BuildSimpleHouse,
        BuildWatchtower,
        BuildFarm,


     */
    private void PerformCommand(Commands command) {
        switch (command) {
            case Commands.CreateWorker:
                PerformCreateWorker();
            break;
            case Commands.CreateWarrior:
                PerformCreateWarrior();
            break;
            case Commands.SendFiveUnitsToAttack:
                PerformSendUnitsToAttack(5);
                break;
            case Commands.BuildGeneral:
                Build(RTSActionType.workersBuildGeneralHouse);
            break;
            case Commands.BuildBarrack:
                Build(RTSActionType.workersBuildBarrack);
            break;

            case Commands.BuildQuarry:
                Build(RTSActionType.workersBuildQuarry);
                break;
            case Commands.BuildMine:
                Build(RTSActionType.workersBuildMine);
                break;
            case Commands.BuildSawmill:
                Build(RTSActionType.workersBuildSawmill);
                break;
            case Commands.BuildForge:
                Build(RTSActionType.workersBuildForge);
                break;
            case Commands.BuildSimpleHouse:
                Build(RTSActionType.workersBuildSimpleHouse);
                break;
            case Commands.BuildWatchtower:
                Build(RTSActionType.workersBuildWatchTower);
                break;
            case Commands.BuildFarm:
                Build(RTSActionType.workersBuildFarm);
                break;
        }
    }




    private void Build(RTSActionType buildActionType) {
        int numberOfWorkers = FindNumberOfWorkers();

        if (numberOfWorkers == 0) {
            globalCommandsQueue.Enqueue(Commands.CreateWorker);
        } else {


//
//
//            Vector3 targetPosition = Vector3.forward;
//
//            AbstractGameUnit worker = ThisArmyManager.FindAnActiveUnitOfType(Identification.UnitType.Worker);
//
//            if (worker != null && worker.IsAvailableFoTasks) {
//
//                if (buildActionType == RTSActionType.workersBuildMine) {
//                    List<Vector3> positions = behaviourScript.MineBuildPoints;
//
//                    targetPosition = FindTheClosestPosition(worker.Avatar.transform.position, positions);
//
//                } else if (buildActionType == RTSActionType.workersBuildQuarry) {
//                    List<Vector3> positions = behaviourScript.QuarryBuildPoints;
//
//                    targetPosition = FindTheClosestPosition(worker.Avatar.transform.position, positions);
//
//                } else {
//                    List<Vector3> positions = behaviourScript.BuildPoints;
//
//                    targetPosition = FindTheClosestFreePosition(worker.Avatar.transform.position, positions);
//                }
//
//
//                if (worker != null) {
//                    ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
//                    ThisArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, Enlist(worker));
//                    ThisArmyDispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction, buildActionType);
//
//                    ThisArmyDispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.setTargetPoint, targetPosition);
//                }
//
//            } // if found active worker //
//

        } // if have workers //
    } // Build() //


    private Vector3 FindTheClosestFreePosition(Vector3 pos, List<Vector3> candidates) {
        Vector3 bestCandidate = candidates[0];
        float bestDistance = (pos - bestCandidate).sqrMagnitude;

        foreach (Vector3 cand in candidates) {
            if( BuildingsConstructionHandler.CanBuildCheck(cand)) {
                float dist = (pos - cand).sqrMagnitude;

                if (dist < bestDistance) {
                    bestCandidate = cand;
                    bestDistance = dist;
                }
            }
        }

        return bestCandidate;
    }


    private Vector3 FindTheClosestPosition(Vector3 pos, List<Vector3> candidates) {
        Vector3 bestCandidate = candidates[0];
        float bestDistance = (pos - bestCandidate).sqrMagnitude;

        foreach (Vector3 cand in candidates) {
            float dist = (pos - cand).sqrMagnitude;

            if (dist < bestDistance) {
                bestCandidate = cand;
                bestDistance = dist;
            }
        }

        return bestCandidate;
    }


    private void PerformSendUnitsToAttack(int number) {

        AbstractGameUnitsList warriors = thisArmyManager.Warriors;
        AbstractGameUnitsList selectedWarriors = null;

        if (warriors.Count <= number) {
            selectedWarriors = warriors;
        } else {
            selectedWarriors = new AbstractGameUnitsList();

            for (int i = 0; i < number; i++) {
                selectedWarriors.Add(warriors[random.Next(0,warriors.Count)]);
            }

        }

        ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
        ThisArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, selectedWarriors);
        ThisArmyDispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction, RTSActionType.attack);

        Vector3 pos = FindRandomEnemy();

        ThisArmyDispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.setTargetPoint, pos);
    }

    private Vector3 FindRandomEnemy() {
        AbstractGameUnitsList enemyList = PlayerArmyManager.Warriors;

        if (enemyList.Count == 0) {
            enemyList = PlayerArmyManager.Buildings;
        }

        AbstractGameUnit randomUnit = enemyList[random.Next(0, enemyList.Count)];

        return randomUnit.Avatar.transform.position;
    }



    private void PerformCreateWarrior() {
        int numberOfGeneralBuildings = FindNumberOfUnitsOfType(Identification.UnitType.Barrack);

        if (numberOfGeneralBuildings == 0) {
            if(FindNumberOfWorkers() > 0) {
                globalCommandsQueue.Enqueue(Commands.BuildBarrack);
            } else {
                globalCommandsQueue.Enqueue(Commands.CreateWorker);
            }
        } else {

            AbstractGameUnit barrack = ThisArmyManager.FindAnActiveUnitOfType(Identification.UnitType.Barrack);

            if (barrack != null && barrack.IsAvailableFoTasks) {
                PerformRandomAction(barrack);
            }

        }
    }

    private void PerformRandomAction(AbstractGameUnit unit) {

        if (unit.IsAvailableFoTasks) {

            int index = random.Next(0, unit.Characteristics.ActionsList.Count);
            RTSActionType randomAction = unit.Characteristics.ActionsList[index];

            ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
            ThisArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, Enlist(unit));


            ThisArmyDispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction, randomAction);
        }
    }

    private void PerformCreateWorker() {

        int numberOfGeneralBuildings = FindNumberOfUnitsOfType(Identification.UnitType.GeneralHouse);

        if (numberOfGeneralBuildings == 0) {
            if(FindNumberOfWorkers() > 0) {
                globalCommandsQueue.Enqueue(Commands.BuildGeneral);
            } else {
                Debug.Log("Can't create workers any more");
            }
        } else {

            AbstractGameUnit general = ThisArmyManager.FindAnActiveUnitOfType(Identification.UnitType.GeneralHouse);

            if (general != null && general.IsAvailableFoTasks) {
                ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.deselectUnits);
                ThisArmyDispatcher.TriggerCommand<AbstractGameUnitsList>(ArmyMessageTypes.addUnitsToSelection, Enlist(general));
                ThisArmyDispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction, RTSActionType.createWorker);
            }
        }
    }


    private AbstractGameUnitsList Enlist(AbstractGameUnit unit) {
        AbstractGameUnitsList list = new AbstractGameUnitsList();
        list.Add(unit);
        return list;
    }

    private int FindNumberOfWorkers() {
        return FindNumberOfUnitsOfType(Identification.UnitType.Worker);
    }


    private int FindNumberOfUnitsOfType (Identification.UnitType type) {
        return thisArmyManager.NumberOfUnitsOfType(type);
    }



///////////////////////////////////////////////////////////////////////////////////////////////////////

    private ArmyManager thisArmyManager;
    public ArmyManager ThisArmyManager {
        get {
            return thisArmyManager;
        }
    }

    private ArmyDispatcher ThisArmyDispatcher {
        get {
            return thisArmyManager.Dispatcher;
        }
    }


    private ArmyDispatcher PlayerArmyDispatcher {
        get {
            return GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].Dispatcher;
        }
    }
    private ArmyManager PlayerArmyManager {
        get {
            return GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy];
        }
    }

//################################################################################################### ////
// Constructor ///////////////////////////////////////////////////////////////////////////////////////////
//################################################################################################### ////

    public AIController(ArmyManager armyManager, AIScript script, GameObject gameObject) {

        this.behaviourScript = script;

        SetArmyManager(armyManager);

        GameResources resources = new GameResources(400, 400, 400, 400);
        thisArmyManager.AvailableResources.AddResources(resources);

        SubscribeOnDispatcherMessages();

        ///

        AIUpdaterComponent component = gameObject.AddComponent<AIUpdaterComponent>();
        component.SetAIController(this);
        component.SetTimePeriod(behaviourScript.TiimePerStep);

////////

        globalCommandsQueue.Enqueue(Commands.CreateWorker);
        globalCommandsQueue.Enqueue(Commands.CreateWorker);

        globalCommandsQueue.Enqueue(Commands.BuildBarrack);

        globalCommandsQueue.Enqueue(Commands.CreateWarrior);
        globalCommandsQueue.Enqueue(Commands.CreateWarrior);
        globalCommandsQueue.Enqueue(Commands.CreateWarrior);
        globalCommandsQueue.Enqueue(Commands.CreateWarrior);
        globalCommandsQueue.Enqueue(Commands.CreateWarrior);

    }


// ################################################################
    public void SetArmyManager(ArmyManager manager) {
        this.thisArmyManager = manager;

        SetUnitsToStandPreparedToFight();
    }

    private void SetUnitsToStandPreparedToFight() {
// Make all units waiting for ocasion to fight //

        foreach(AbstractGameUnit unit in thisArmyManager.Warriors) {
            ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.unitCommandGoToStandPreparedState, unit.ID);
        }

    }
// ###############################################################


    private void SubscribeOnDispatcherMessages() {

        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked,
                (AbstractGameUnit unit) => {
                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("AI Controller:: Unit is dead: " + unit.Description);

                    thisArmyManager.DestroyGameUnit(unit.ID);
                }
        );

    } // Subscribe on dispatcher messages //



// Send message to PlayerController to tell it "some other unit is clicked" //
    private void TellPlayerThatUnitWasClicked(AbstractGameUnit unit) {
        Debug.Log("AI:: Sending message about a click to player");
        GameManager.Instance.PlayerController.NewTargetUnit(unit);
    }




}