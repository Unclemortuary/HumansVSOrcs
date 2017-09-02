using UnityEngine;
using UnityEngine.AI;

public class UnitStateMachine : RTSMonoBehaviour {


    private UnitStateMachineHelper helper;
    public UnitStateMachineHelper Helper {
        get {
            return helper;
        }
    }

    public void SetUnitStateMachineHelper(UnitStateMachineHelper helper) {
        this.helper = helper;

        ////////////////////////////////////


        SubscribeOnDispatcherMessages();

    }



    private void SubscribeOnDispatcherMessages() {

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandGoToStandPreparedState, () => {
            GoToStandPreparedState();
        }, helper.ThisUnit.ID);


        armyManager.Dispatcher.StartListening<bool>(ArmyMessageTypes.unitCommandTurnSelection,
            (bool on) => {
                helper.TurnSelection(on);
            },
            helper.ThisUnit.ID
        );


        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToPosition,
            (Vector3 pos) => {
                Debug.Log("%% init go to point action %%");
                GoToWalkingToPointState(pos);
            },
            helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
            () => {
                GoToIdleState();
            },
            helper.ThisUnit.ID
        );



        //////// builder ////////////////////////////////


        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandBuild,
                (AbstractGameUnit building) => {

                    GoToGoingToBuildState(building);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCommandCreateUnit,
                (AbstractGameUnit newUnit) => {

                    GoToCreatingUnitState(newUnit);
                },
                helper.ThisUnit.ID
        );

        armyManager.Dispatcher.StartListening<float>(ArmyMessageTypes.unitCommandSetWorkDuration,
                (float time) => {
                    helper.TaskDuration = time;
                },
                helper.ThisUnit.ID
        );


    } // SubscribeOnDispatcherMessages() //




    //////////////////////////////////////////////////////////////////////////////////////////////////
    // ******************************************************************************************** //
    //////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////
    // ******************************************************************************************** //
    //////////////////////////////////////////////////////////////////////////////////////////////////


    public enum State {
        IDLE, // standing still, going to attack if was attacked
        STAND_PREPARED, // stand, going to attack if sees enemy
        HOLD_POSITION, // stand, attacks enemy if sees him, but do not go far away, returns when has nobody to attack
        WALKING_TO_POINT, // goint to target point, attacks if was attacked
        MOVE_AND_ATTACK, // moving to a point, attacks any enemy on the way
        FOLLOW_AND_ATTACK, // following a unit, trying to attack him

        GOING_TO_BUILD, // moving to a point to build a building
        BUILDING, // waiting building proccess to be complete

        CREATING_UNIT,

        IS_DEAD,
    }


    private State currentState = State.IDLE;
    public State CurrentState {
        get {
            return currentState;
        }
    }



    void Update() {

        TestHealth();

        switch (CurrentState) {
            case State.IDLE :
                IdleState();
            break;
            case State.STAND_PREPARED :
                StandPreparedState();
            break;
            case State.HOLD_POSITION :
                HoldPositionState();
            break;
            case State.WALKING_TO_POINT :
                WalkingToDestinationPointState();
            break;
            case State.MOVE_AND_ATTACK :
                MoveAndAttackState();
            break;
            case State.FOLLOW_AND_ATTACK :
                FollowAndAttackState();
            break;
            case State.GOING_TO_BUILD :
                GoingToBuildState();
            break;
            case State.BUILDING :
                BuildingState();
            break;
            case State.CREATING_UNIT :
                CreatingUnitState();
            break;
            case State.IS_DEAD :
                IsDeadState();
            break;
        }

    }


// ################################################################################
    private void TestHealth() {

    }


// ################################################################################
// ################################################################################


// ################################################################################
    private void GoToIdleState() {

        if (helper.ThisUnit.IsActive) {

            if (helper.Agent != null) {
                helper.Agent.ResetPath();
            }

            Debug.Log(">>> goto Idle State <<<");
            currentState = State.IDLE;
        }
    }

    private void IdleState() {

        // Attack if attacked //


    }

// ################################################################################
    private void GoToStandPreparedState() {
        if(helper.ThisUnit.IsActive) {



            Debug.Log(">>> goto Stand Prepared State <<<");
            currentState = State.STAND_PREPARED;
        }
    }

    private void StandPreparedState() {


    }

// ################################################################################
    private void GoToHoldPositionState() {

        if (helper.ThisUnit.IsActive) {



            Debug.Log(">>> goto Hold Position State <<<");
            currentState = State.HOLD_POSITION;
        }

    }

    private void HoldPositionState() {



    }

// ################################################################################
    private void GoToWalkingToPointState(Vector3 pos) {

        if (helper.ThisUnit.IsActive && helper.Agent != null) {

            helper.Agent.destination = pos;

            Debug.Log(">>> goto Walking to Point State <<<  new destination=" + helper.Agent.destination);
            currentState = State.WALKING_TO_POINT;
        }
    }

    private void WalkingToDestinationPointState() {


//        Debug.Log("<<<  velocity=" + helper.Agent.velocity + " remainingDistance = " + helper.Agent.remainingDistance + " delta = " + helper.Agent.stoppingDistance);
//        if (helper.Agent.remainingDistance <= helper.Agent.stoppingDistance) {
        if ((helper.Agent.destination - transform.position).magnitude <= helper.Agent.stoppingDistance) {
            GoToIdleState();
        }

    }

// ################################################################################
    private void GoToMoveAndAttackState() {

        if (helper.ThisUnit.IsActive) {




            Debug.Log(">>> goto Move and Attack State <<<");
            currentState = State.MOVE_AND_ATTACK;
        }
    }

    private void MoveAndAttackState() {


    }

// ################################################################################
    private void GoToFollowAndAttackState() {

        if (helper.ThisUnit.IsActive) {




            Debug.Log(">>> goto Follow and Attack State <<<");
            currentState = State.FOLLOW_AND_ATTACK;
        }
    }

    private void FollowAndAttackState() {


    }

// ################################################################################
    private void GoToGoingToBuildState(AbstractGameUnit building) {

        if (helper.ThisUnit.IsActive) {


            helper.TargetUnit = building;

            SetActivityTo(false);

            ///////// Adjast miniimum distance to build ////////
            Vector3 buildingExtents = helper.TargetUnit.Avatar.GetComponent<Collider>().bounds.extents;
            helper.WorkingRadius = buildingExtents.x * buildingExtents.x + buildingExtents.z * buildingExtents.z;

            Vector3 obstacleSize = helper.TargetUnit.Avatar.GetComponent<NavMeshObstacle>().size;
            float navmeshRadius = obstacleSize.x * obstacleSize.x * 0.25f +
                                    obstacleSize.z * obstacleSize.z * 0.25f;

            if (helper.WorkingRadius < navmeshRadius) {
                helper.WorkingRadius = navmeshRadius;
            }

            helper.WorkingRadius += helper.StoppingDistance + helper.WorkingRadiusDelta;
            //////////////////////////////////////////////////////

            helper.Agent.destination = helper.TargetUnit.Avatar.transform.position;

            Debug.Log(">>> goto Going to Build State <<<");
            currentState = State.GOING_TO_BUILD;
        }

    }

    private void GoingToBuildState() {

        if ((transform.position - helper.TargetUnit.Avatar.transform.position).sqrMagnitude > helper.WorkingRadius) {
//            Debug.Log("distance^2=" + (transform.position - helper.TargetBuilding.Avatar.transform.position).sqrMagnitude +
//            ", buildingDistance^2=" + helper.WorkingRadius);
        } else {

            helper.TargetUnit.Avatar.GetComponent<BuildingComponent>().SetTransparent(false);

            helper.TargetBuildingScaffold = helper.TargetUnit.Avatar.transform.Find("BuildingBuild").gameObject;


            if (helper.TargetBuildingScaffold != null) {
                Debug.Log("scaffold is found");
            } else {
                Debug.Log("scaffold is not found");
            }

            Debug.Log("Setting scaffold active");
            helper.TargetBuildingScaffold.SetActive(true);


            helper.Agent.ResetPath();



            currentState = State.BUILDING;

            helper.TaskRemaintinTime = helper.TaskDuration;
            helper.TaskName = currentState.ToString();
        }

    }


    private void BuildingState() {

        Debug.Log("*** Task Remaining Time = " + helper.TaskRemaintinTime + ", deltatime=" + Time.deltaTime);

        helper.TaskRemaintinTime -= Time.deltaTime;

        if ( helper.TaskRemaintinTime > 0) {

        } else {

            helper.TargetBuildingScaffold.SetActive(false);


            SetActivityTo(true);


            ////////////////////////////////////
            Identification.UnitType type = armyManager.GetBuildingTypeByAbstractGameUnit(helper.TargetUnit);
            if (type == Identification.UnitType.GeneralHouse) {
                armyManager.AvailableResources.ChangeResourceAmount(
                        GameResources.ResourceType.GENERAL_HOUSES, 1);

            } else if (type == Identification.UnitType.SimpleHouse) {
                armyManager.AvailableResources.ChangeResourceAmount(
                        GameResources.ResourceType.LIVING_HOUSES, 1);
            }
            ///////////////////////////////////


            GoToIdleState();
        }



    }



    private void SetActivityTo(bool val) {
        Debug.Log("##### Set Activity to " + val.ToString());

        helper.TargetUnit.IsActive = val;
        helper.ThisUnit.IsActive = val;
        armyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
    }

// ################################################################################
    private void GoToCreatingUnitState(AbstractGameUnit newUnit) {

        if (helper.ThisUnit.IsActive) {

            helper.TargetUnit = newUnit;

            SetActivityTo(false);

            Debug.Log(">>> goto Creating Unit State <<<");
            currentState = State.CREATING_UNIT;

            helper.TaskRemaintinTime = helper.TaskDuration;
            helper.TaskName = currentState.ToString();
        }
    }

    private void CreatingUnitState() {

        Debug.Log("*** Task Remaining Time = " + helper.TaskRemaintinTime + ", deltatime=" + Time.deltaTime);

        helper.TaskRemaintinTime -= Time.deltaTime;

        if ( helper.TaskRemaintinTime > 0) {

        } else {

            helper.TargetUnit.Avatar.SetActive(true);

            SetActivityTo(true);

            ////////////////////////////////
            armyManager.AvailableResources.ChangeResourceAmount(GameResources.ResourceType.MEN, 1);
            ////////////////////////////////


            ///// Find out where to move unit to ///////////////////////////////
            Vector3 targetPoint = helper.TargetUnit.Avatar.transform.position;
            Vector3 shiftVector = targetPoint - transform.position;

            targetPoint = targetPoint + 4 * shiftVector.normalized;

            Debug.Log("SamplePosition?");
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPoint, out hit, 5.0f, NavMesh.AllAreas)) {
                Debug.Log("Position is " + hit.position);
                targetPoint = hit.position;
            } else {
                Debug.Log("SamplePosition = false");
            }

            // Send new unit to walk a little forward //
            armyManager.Dispatcher.TriggerCommand<Vector3>(
                    ArmyMessageTypes.unitCommandGoToPosition, targetPoint,
                    helper.TargetUnit.ID
            );


            // Make this building (unit creator) IDLE //
            GoToIdleState();


        }



    }// Creating Unit State //




// ################################################################################
    private void GoToIsDeadState() {



        Debug.Log(">>> goto IsDead State <<<");
        currentState = State.IS_DEAD;
    }

    private void IsDeadState() {
        armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead, helper.ThisUnit);
    }


// ################################################################################
// ################################################################################


} // End of class //

