using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class UnitStateMachine : RTSMonoBehaviour {

    [SerializeField]
    private UnitStateMachineHelper helper;
    public IEnemyHelper EnemyHelper {
        get {
            return helper;
        }
    }
    public ITaskerHelper TaskerHelper {
        get {
            return helper;
        }
    }


//    [SerializeField]
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
        COOLDOWN, // pause between strikes //
//        ATTACKING, // attack at regular intervals

        GOING_TO_BUILD, // moving to a point to build a building
        BUILDING, // waiting building proccess to be complete

        CREATING_UNIT, // waiting process of creation (showing progress) to be finished, then create unit

        IS_DEAD,
    }


    [SerializeField]
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
            case State.COOLDOWN :
                CooldownState();
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

        if (helper.ThisUnit.IsAvailableFoTasks) {

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

        if (helper.ThisUnit.IsAvailableFoTasks && helper.Agent != null) {

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
        if(helper.ThisUnit.IsAvailableFoTasks) {
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

        if (helper.ThisUnit.IsAvailableFoTasks && helper.Agent != null) {

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

        if(stateMachine != null && stateMachine.EnemyHelper != null) {
            TransitionToFollowAndAttackState(stateMachine.EnemyHelper);
        }

    }

    public void TransitionToFollowAndAttackState(IEnemyHelper enemyHelper) {

        if (helper.ThisUnit.IsAvailableFoTasks) {

            helper.TargetEnemyHelper = enemyHelper;

//            Debug.Log(">>> goto Follow and Attack State <<< unitID=" + helper.ThisUnit.ID
//                        + " following unit of army " + enemyHelper.MyArmy + " at position " + enemyHelper.GetPosition());

            currentState = State.FOLLOW_AND_ATTACK;
        }
    }

    private void FollowAndAttackState() {


        IEnemyHelper enemy = helper.TargetEnemyHelper;

        if (enemy == null || !enemy.IsAlife()) {
            TransitionToStandPreparedState();
        } else {

            Vector3 myPosition = helper.ThisUnit.Avatar.transform.position;
            float viewDistance = helper.ThisUnit.Characteristics.MaxViewDistance;
            float attackDistance = helper.ThisUnit.Characteristics.MaxAttackDistance
                                + helper.TargetEnemyHelper.GetRadius() + helper.GetRadius();;

            Vector3 enemyPosition = helper.TargetEnemyHelper.GetPosition();

            Vector3 viewToEnemyDirection = enemyPosition - myPosition;
            float distanceToEnemy = viewToEnemyDirection.magnitude;

            // look rotateion
            helper.ThisUnit.Avatar.transform.rotation = Quaternion.LookRotation(viewToEnemyDirection);


            if (distanceToEnemy > viewDistance) {
                // lost him, try to find //
                TransitionToMoveAndAttackState(enemyPosition);
            } else if (distanceToEnemy > attackDistance) {
                // go to him //
                helper.Agent.destination = enemyPosition;
            } else {
                // attack him //
                helper.Agent.ResetPath();

                // Animate strike //
                if (helper.Animator != null) {
                    helper.Animator.SetTrigger(helper.ATTACK_HASH);
                } else {
                    Debug.Log("Can't animate strike: Aniimator is null");
                }
                // Damage him //
                float drop = enemy.DamageHim(helper.ThisUnit.Characteristics.AttackPhisDamage);

                if (drop != 0) {
                    GameManager.Instance.ArmyManagers[helper.MyArmy].AvailableResources
                                .ChangeResourceAmount(GameResources.ResourceType.GOLD, drop);
                }

                // Make pause //
                TransitionToCooldownState();
//                TransitionToAttackingState();
            }

        } // if enemy is alife //

    }

// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

    private void TransitionToCooldownState() {
        if (helper.ThisUnit.IsAvailableFoTasks) {

            currentState = State.COOLDOWN;

            helper.TaskRemaintinTime = helper.ThisUnit.Characteristics.AttackCooldownTime;
            helper.TaskName = currentState.ToString();
        }
    }

    private void CooldownState() {
//            Debug.Log("COOLDOWN:: *** Task Remaining Time = " + helper.TaskRemaintinTime + ", deltatime=" + Time.deltaTime);

            helper.TaskRemaintinTime -= Time.deltaTime;

            if ( helper.TaskRemaintinTime <= 0) {
                TransitionToFollowAndAttackState(helper.TargetEnemyHelper);
            }
    }

//    private void TransitionToAttackingState() {
//        Debug.Log(">>> goto ATTACKING State <<< unitID=" + helper.ThisUnit.ID
//        + " following unit of army " + helper.TargetEnemyHelper.MyArmy
//        + " at position " + helper.TargetEnemyHelper.GetPosition());
//
//        currentState = State.ATTACKING;
//    }
//
//    private void AttackingState() {
////        MaxAttackDistance
////        MaxViewDistance
////        AttackPhisDamage
////        AttackCooldownTime
//
//
//        IEnemyHelper enemy = helper.TargetEnemyHelper;
//
//        if (enemy == null || !enemy.IsAlife()) {
//            TransitionToStandPreparedState();
//        } else {
//
//            Vector3 myPosition = helper.ThisUnit.Avatar.transform.position;
//            float attackDistance = helper.ThisUnit.Characteristics.MaxAttackDistance;
//
//            Vector3 enemyPosition = helper.TargetEnemyHelper.GetPosition();
//
//            Vector3 viewToEnemyDirection = enemyPosition - myPosition;
//            float distanceToEnemy = viewToEnemyDirection.magnitude;
//
//            // look rotateion
//            helper.ThisUnit.Avatar.transform.rotation = Quaternion.LookRotation(viewToEnemyDirection);
//
//
//            if (distanceToEnemy <= attackDistance) {
//                //asdf;
//                enemy.DamageHim(helper.ThisUnit.Characteristics.AttackPhisDamage);
//
//            } else {
//
//                TransitionToFollowAndAttackState(helper.TargetEnemyHelper);
//            }
//
//        } // if enemy is alife //
//
//    }

// ################################################################################
    public void TransitionToHoldPositionState() {

        if (helper.ThisUnit.IsAvailableFoTasks) {



            Debug.Log(">>> goto Hold Position State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.HOLD_POSITION;
        }

    }

    private void HoldPositionState() {



    }

// ################################################################################
    public void TransitionToGoingToBuildState(AbstractGameUnit building) {

        if (helper.ThisUnit.IsAvailableFoTasks) {


            helper.TargetUnit = building;

            DeactivateUnitsWhileCreatingTo(false);

//            ///////// Adjast miniimum distance to build ////////
//            Vector3 buildingExtents = helper.TargetUnit.Avatar.GetComponent<Collider>().bounds.extents;
//            helper.WorkingRadius = buildingExtents.x * buildingExtents.x + buildingExtents.z * buildingExtents.z;
//
//            Vector3 obstacleSize = helper.TargetUnit.Avatar.GetComponent<NavMeshObstacle>().size;
//            float navmeshRadius = obstacleSize.x * obstacleSize.x * 0.25f +
//                                    obstacleSize.z * obstacleSize.z * 0.25f;
//
//            if (helper.WorkingRadius < navmeshRadius) {
//                helper.WorkingRadius = navmeshRadius;
//            }

            IEnemyHelper buildingHelper = building.Avatar.GetComponent<UnitStateMachine>().EnemyHelper;

            helper.WorkingRadius = buildingHelper.GetRadius() + helper.GetRadius();

//            helper.WorkingRadius += helper.WorkingRadiusDelta;

//            helper.WorkingRadius += helper.StoppingDistance + helper.WorkingRadiusDelta;
            //////////////////////////////////////////////////////

            helper.Agent.destination = helper.TargetUnit.Avatar.transform.position;

            Debug.Log(">>> goto Going to Build State <<< unitID=" + helper.ThisUnit.ID);
            currentState = State.GOING_TO_BUILD;
        }

    }

    private void GoingToBuildState() {

        if ((transform.position - helper.TargetUnit.Avatar.transform.position).magnitude > helper.WorkingRadius) {
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


            // Stop //
            helper.Agent.ResetPath();


            // Go to building process state //
            currentState = State.BUILDING;

            // Start counting progress of building process //
            helper.TaskRemaintinTime = helper.TaskDuration;
            helper.TaskName = currentState.ToString();

            // Set component to show construction progress on building //
            BuildingProgressComponent progressComponent = helper.TargetUnit.Avatar.AddComponent<BuildingProgressComponent>();
            progressComponent.Helper = helper;

            // Обновить отображаемую в HUD информацию //
            armyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
        }

    }


    private void BuildingState() {

//        Debug.Log("*** Task Remaining Time = " + helper.TaskRemaintinTime + " of " + helper.TaskDuration + ", deltatime=" + Time.deltaTime);

        helper.TaskRemaintinTime -= Time.deltaTime;

        if ( helper.TaskRemaintinTime > 0) {

        } else {

            helper.TargetBuildingScaffold.SetActive(false);


            DeactivateUnitsWhileCreatingTo(true);


            ////////////////////////////////////
            Identification.UnitType type = armyManager.GetTypeByAbstractGameUnit(helper.TargetUnit);
            if (type == Identification.UnitType.GeneralHouse) {
                armyManager.AvailableResources.ChangeResourceAmount(
                        GameResources.ResourceType.GENERAL_HOUSES, 1);

            } else if (type == Identification.UnitType.SimpleHouse) {
                armyManager.AvailableResources.ChangeResourceAmount(
                        GameResources.ResourceType.LIVING_HOUSES, 1);
            }
            ///////////////////////////////////

			ResourceBuilding resourceBuilding = helper.TargetUnit.Avatar.GetComponent<ResourceBuilding> ();
			if (resourceBuilding != null)
			{
				resourceBuilding.StartMining ();
				Debug.Log ("Start mining " + resourceBuilding.ResourceType.ToString());
			}


            TransitionToIdleState();
        }



    }


    // Fol Build an Create actions //
    // Makes inactive current unit and target unit //
    private void DeactivateUnitsWhileCreatingTo(bool val) {
        Debug.Log("##### Set Activity to " + val.ToString());

        if (helper.TargetUnit != null) {
            helper.TargetUnit.IsAvailableFoTasks = val;
        }
        helper.ThisUnit.IsAvailableFoTasks = val;
        armyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.refreshSelection);
    }

// ################################################################################
    public void TransitionToCreatingUnitState(AbstractGameUnit newUnit) {

        if (helper.ThisUnit.IsAvailableFoTasks) {

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

//        Debug.Log("*** Task Remaining Time = " + helper.TaskRemaintinTime + ", deltatime=" + Time.deltaTime);

        helper.TaskRemaintinTime -= Time.deltaTime;

        if ( helper.TaskRemaintinTime > 0) {

        } else { // Time has come, Create unit //

            if(helper.TargetUnit == null) {
                Debug.Log("USM::CreatingUnitState:: target unit is null, thisUnitID=" + helper.ThisUnit.ID);
            }
            Debug.Log("USM::CreatingUnitStae:: helper.targetUnit.id=" + helper.TargetUnit.ID);
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

            Debug.Log("##USM::CreatingUnitState:: Unit is set visible and sent to a position, myID=" + helper.ThisUnit.ID);

            // Make this building (unit creator) IDLE //
            TransitionToIdleState();

        } //  unit creating //

    }// Creating Unit State //




// ################################################################################
    public void TransitionToIsDeadState() {

        helper.ThisUnit.IsAvailableFoTasks = false;

        armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead, helper.ThisUnit);

        Debug.Log(">>> goto IsDead State <<< unitID=" + helper.ThisUnit.ID);
        currentState = State.IS_DEAD;
    }

    private void IsDeadState() {

    }


// ################################################################################
// ################################################################################


} // End of class //

