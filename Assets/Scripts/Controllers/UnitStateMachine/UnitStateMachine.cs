using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class UnitStateMachine : RTSMonoBehaviour {


    private UnitStateMachineHelper helper;
    public IEnemyHelper Helper {
        get {
            return helper;
        }
    }

    private UnitStateMachineSubscriptions subscriptions;

    public void SetUnitStateMachineHelper(UnitStateMachineHelper helper) {
        this.helper = helper;

        this.subscriptions = new UnitStateMachineSubscriptions(this, helper, armyManager);

        TransitionToIdleState();
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////
    // ******************************************************************************************** //
    //////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////
    // ******************************************************************************************** //
    //////////////////////////////////////////////////////////////////////////////////////////////////


    public enum State {
        _NONE,
        IDLE, // standing still, going to attack if was attacked
        STAND_PREPARED, // stand, going to attack if sees enemy
        HOLD_POSITION, // stand, attacks enemy if sees him, but do not go far away, returns when has nobody to attack
        WALKING_TO_POINT, // goint to target point, attacks if was attacked
        MOVE_AND_ATTACK, // moving to a point, attacks any enemy on the way
        FOLLOW_AND_ATTACK, // following a unit, trying to attack him

        GOING_TO_BUILD, // moving to a point to build a building
        BUILDING, // waiting building proccess to be complete

        CREATING_UNIT, // waiting process of creation (showing progress) to be finished, then create unit

        IS_DEAD,
    }


    private State currentState = State._NONE;
    public State CurrentState {
        get {
            return currentState;
        }
    }



    void Update() {

//        if(helper == null) {
//            Debug.Log("UnitStateMachine Update:: helper is null, currentState is " + CurrentState.ToString());
//        } else {
//            Debug.Log("UnitStateMachine Update:: helper is ok");
//        }

        if (CurrentState != State._NONE) {

            if (!helper.IsAlife() && CurrentState != State.IS_DEAD) {
                TransitionToIsDeadState();
            }


            ExecuteCurrentState();
        }

    }

    private void ExecuteCurrentState() {
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
// ################################################################################


// ################################################################################
    public void TransitionToIdleState() {

        if (helper.ThisUnit.IsActive) {

            if (helper.Agent != null) {
                helper.Agent.ResetPath();
            }

            helper.DropAttackedFlag();

            if(helper == null) {
                Debug.Log("GoToIdleState:: helper is null");
            }

            Debug.Log(">>> goto Idle State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.IDLE;
        }
    }

    private void IdleState() {

//        if(helper == null) {
//            Debug.Log("IdleState:: helper is null");
//        }

        // Attack if attacked //
        if (helper.WasAttacked) {
            TransitionToStandPreparedState();
        }
    }


// ################################################################################
    public void TransitionToWalkingToPointState(Vector3 pos) {

        if (helper.ThisUnit.IsActive && helper.Agent != null) {

            helper.Agent.destination = pos;

            Debug.Log(">>> goto Walking to Point State <<<  new destination=" + helper.Agent.destination + " unitID=" + helper.ThisUnit.ID);
            currentState = State.WALKING_TO_POINT;
        }
    }

    private void WalkingToDestinationPointState() {

        if (helper.WasAttacked) {
            TransitionToMoveAndAttackState(helper.Agent.destination);
        } else if ((helper.Agent.destination - transform.position).magnitude <= helper.Agent.stoppingDistance) {
            TransitionToIdleState();
        }

    }

// ################################################################################
    public void TransitionToStandPreparedState() {
        if(helper.ThisUnit.IsActive) {
            Debug.Log(">>> goto Stand Prepared State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.STAND_PREPARED;
        }
    }

    private void StandPreparedState() {

        Vector3 myPosition = helper.ThisUnit.Avatar.transform.position;
        float viewDistance = helper.ThisUnit.Characteristics.MaxViewDistance;

        IEnemyHelper closestEnemy = helper.FindTheClosestEnemy(myPosition, viewDistance);

        if (closestEnemy != null) {
            TransitionToFollowAndAttackState(closestEnemy);
        }

    }  // Stand prepared State //

// ################################################################################
    public void TransitionToMoveAndAttackState(Vector3 attackPoint) {

        if (helper.ThisUnit.IsActive && helper.Agent != null) {

            helper.Agent.destination = attackPoint;

            Debug.Log(">>> goto Move and Attack State <<< unitID=" + helper.ThisUnit.ID
                        + ", attackPoint=" + attackPoint);

            currentState = State.MOVE_AND_ATTACK;
        }
    }

    private void MoveAndAttackState() {

        Vector3 myPosition = helper.ThisUnit.Avatar.transform.position;
        float viewDistance = helper.ThisUnit.Characteristics.MaxViewDistance;

        IEnemyHelper closestEnemy = helper.FindTheClosestEnemy(myPosition, viewDistance);

        if (closestEnemy != null) {
            // the closest enemy found ? //
            TransitionToFollowAndAttackState(closestEnemy);
        } else if ((helper.Agent.destination - transform.position).magnitude <= helper.Agent.stoppingDistance) {
            TransitionToStandPreparedState();
        }

    } // Move and attack State //


// ################################################################################
    public void TransitionToFollowAndAttackState(AbstractGameUnit unit) {
        UnitStateMachine stateMachine = unit.Avatar.GetComponent<UnitStateMachine>();

        if(stateMachine != null && stateMachine.Helper != null) {
            TransitionToFollowAndAttackState(stateMachine.Helper);
        }

    }

    public void TransitionToFollowAndAttackState(IEnemyHelper enemyHelper) {

        if (helper.ThisUnit.IsActive) {

            helper.TargetEnemyHelper = enemyHelper;

            Debug.Log(">>> goto Follow and Attack State <<< unitID=" + helper.ThisUnit.ID
                        + " following unit of army " + enemyHelper.MyArmy + " at position " + enemyHelper.GetPosition());

            currentState = State.FOLLOW_AND_ATTACK;
        }
    }

    private void FollowAndAttackState() {


//        MaxAttackDistance
//        MaxViewDistance
//        AttackPhisDamage
//        AttackCooldownTime

        IEnemyHelper enemy = helper.TargetEnemyHelper;
        Vector3 enemyPosition = helper.TargetEnemyHelper.GetPosition();

        Vector3 enemyView = enemyPosition - transform.position;
        float distanceToEnemy = enemyView.magnitude;

        // look rotateion

        // if < attackdist then destination - enemyPosition

        // if > viewDsit then Stand Prepared

        // else attack!!!




//  if ((helper.Agent.destination - transform.position).magnitude <= helper.Agent.stoppingDistance) {


    }


// ################################################################################
    public void TransitionToHoldPositionState() {

        if (helper.ThisUnit.IsActive) {



            Debug.Log(">>> goto Hold Position State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.HOLD_POSITION;
        }

    }

    private void HoldPositionState() {



    }

// ################################################################################
    public void TransitionToGoingToBuildState(AbstractGameUnit building) {

        if (helper.ThisUnit.IsActive) {


            helper.TargetUnit = building;

            DeactivateUnitsWhileCreatingTo(false);

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

            Debug.Log(">>> goto Going to Build State <<< unitID=" + helper.ThisUnit.ID);
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


            DeactivateUnitsWhileCreatingTo(true);


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


            TransitionToIdleState();
        }



    }


    // Fol Build an Create actions //
    // Makes inactive current unit and target unit //
    private void DeactivateUnitsWhileCreatingTo(bool val) {
        Debug.Log("##### Set Activity to " + val.ToString());

        helper.TargetUnit.IsActive = val;
        helper.ThisUnit.IsActive = val;
        armyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
    }

// ################################################################################
    public void TransitionToCreatingUnitState(AbstractGameUnit newUnit) {

        if (helper.ThisUnit.IsActive) {

            helper.TargetUnit = newUnit;

            DeactivateUnitsWhileCreatingTo(false);

            Debug.Log(">>> goto Creating Unit State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.CREATING_UNIT;

            helper.TaskRemaintinTime = helper.TaskDuration;
            helper.TaskName = currentState.ToString();

            ////////////////////////////////
            armyManager.AvailableResources.ChangeResourceAmount(GameResources.ResourceType.MEN, 1);
            ////////////////////////////////
        }
    }

    private void CreatingUnitState() {

        Debug.Log("*** Task Remaining Time = " + helper.TaskRemaintinTime + ", deltatime=" + Time.deltaTime);

        helper.TaskRemaintinTime -= Time.deltaTime;

        if ( helper.TaskRemaintinTime > 0) {

        } else { // Time has come, Create unit //

            helper.TargetUnit.Avatar.SetActive(true);

            DeactivateUnitsWhileCreatingTo(true);



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
            TransitionToIdleState();

        } //  unit creating //

    }// Creating Unit State //




// ################################################################################
    public void TransitionToIsDeadState() {

        helper.ThisUnit.IsActive = false;

        armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead, helper.ThisUnit);

        Debug.Log(">>> goto IsDead State <<< unitID=" + helper.ThisUnit.ID);
        currentState = State.IS_DEAD;
    }

    private void IsDeadState() {

    }


// ################################################################################
// ################################################################################


} // End of class //

