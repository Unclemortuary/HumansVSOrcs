using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitStateMachineHelper : IEnemyHelper, ITaskerHelper {

// Selection light circle //
    private GameObject projector;


    private AbstractGameUnit thisUnit;
    public AbstractGameUnit ThisUnit {
        get {
            return thisUnit;
        }
    }

    private NavMeshAgent agent;
    public NavMeshAgent Agent {
        get {
            return agent;
        }
    }
    private float stoppingDistance  = 2;
    public float StoppingDistance {
        get {
            return stoppingDistance;
        }
    }


    private ArmyManager armyManager;

    public Identification.Army MyArmy {
        get {
            return armyManager.ThisArmy;
        }
    }



    /////////////////////////

    public UnitStateMachineHelper(AbstractGameUnit unit, NavMeshAgent agent, ArmyManager manager) {

        this.thisUnit = unit;

        this.agent = agent;
        agent.stoppingDistance = stoppingDistance;

        this.armyManager = manager;

        // ------------

        projector = thisUnit.Avatar.transform.Find("Projector").gameObject;

        InitializeEventTrigger();

    }


    public UnitStateMachineHelper(AbstractGameUnit unit, ArmyManager manager) {

        this.thisUnit = unit;

        this.armyManager = manager;

        // ------------

        projector = thisUnit.Avatar.transform.Find("Projector").gameObject;

        InitializeEventTrigger();

    }




    private void InitializeEventTrigger() {

        EventTrigger eventTrigger = ThisUnit.Avatar.GetComponent<EventTrigger>( );

        if (eventTrigger == null) {
            eventTrigger = ThisUnit.Avatar.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry( );

        entry.eventID = EventTriggerType.PointerDown;

        entry.callback.AddListener( ( data ) => { IAmClicked( (PointerEventData)data ); } );

        eventTrigger.triggers.Add( entry );


    } // InitializeEventTrigger() //




    public void IAmClicked(PointerEventData data) {

        if (data.button == PointerEventData.InputButton.Left)
        {
            armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked, thisUnit);
        }
        else if (data.button == PointerEventData.InputButton.Right)
        {
            armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked, thisUnit);
        }
    }



    public void TurnSelection(bool on) {
        if(IsAlife()) {
            projector.SetActive(on);
        }
    }


    ///////////////////////////////////////////////////////////////
    // --------------------------------------------------------- //
    // ********************************************************* //
    ///////////////////////////////////////////////////////////////

    // Building ///////////////////////////////////////////////////



    private float workingRadius = 225f; //15;
    private float workingRadiusDelta = 3f;

    private AbstractGameUnit targetUnit;


    private float workTaskDuration = 3f;


    public float WorkingRadius {
        get {
            return workingRadius;
        }
        set {
            workingRadius = value;
        }
    }

    public float WorkingRadiusDelta {
        get {
            return workingRadiusDelta;
        }
    }

    public AbstractGameUnit TargetUnit {
        get {
            return targetUnit;
        }
        set {
            targetUnit = value;
        }
    }

    // /// //
    public float TaskDuration {
        get {
            return workTaskDuration;
        }
        set {
            workTaskDuration = value;
        }
    }
    public float TaskRemaintinTime { get; set; }
    public string TaskName { set; get; }
    // /// //

    public GameObject TargetBuildingScaffold { get; set; }

    ///////////////////////////////////////////////////////////////
    // --------------------------------------------------------- //
    // ********************************************************* //
    ///////////////////////////////////////////////////////////////

    // Fighting ///////////////////////////////////////////////////


    public IEnemyHelper TargetEnemyHelper { get; set; }

    // The unit was attaeked flag //
    private bool wasAttacked = false;
    public bool WasAttacked {
        get {
            if (wasAttacked) {
                wasAttacked = false;
                return true;
            }
            return false;
        }
    }
    public void DropAttackedFlag() {
        wasAttacked = false;
    }


    public bool IsAlife() {
        if (ThisUnit == null || ThisUnit.IsDead()) {
            return false;
        }

        return true;
    }

    public void DamageHim(float damageValue) {
        ThisUnit.ChangeHP((ThisUnit.Characteristics.Defence - 1) * damageValue);

        wasAttacked = true;
    }

    public Vector3 GetPosition() {
        if(ThisUnit == null) {
            Debug.Log("USMH:: this unit is null");
        }
        if(ThisUnit.Avatar == null) {
            Debug.Log("USMH:: avatar is null");
            Debug.Log("HP=" + ThisUnit.CurrentHP + ", is alife=" + IsAlife().ToString());
        }
        if(ThisUnit.Avatar.transform == null) {
            Debug.Log("USMH:: transform is null");
        }
        if(ThisUnit.Avatar.transform.position == null) {
            Debug.Log("USMH:: position is null");
        }

        return ThisUnit.Avatar.transform.position;
    }

    public IEnemyHelper FindTheClosestEnemy(Vector3 myPosition, float viewDistance) {
        List<IEnemyHelper> enemyHelpersList = CheckEnemies(
                myPosition,
                viewDistance
        );


        if (enemyHelpersList.Count > 0) {
            float minDistSqr = viewDistance * viewDistance + 1;
            IEnemyHelper closestEnemy = enemyHelpersList[0];

            // find closest enemy //
            foreach (IEnemyHelper unitSMH in enemyHelpersList) {
                float distToHimSqr = (myPosition - unitSMH.GetPosition()).sqrMagnitude;
                if (distToHimSqr < minDistSqr) {
                    minDistSqr = distToHimSqr;
                    closestEnemy = unitSMH;
                }
            }

            // the closest enemy found //
            return closestEnemy;

        } else {
            // No enemies found //
            return null;
        }
    } // Find the closest enemy() //

    public List<IEnemyHelper> CheckEnemies(Vector3 center, float radius) {


        List<IEnemyHelper> unitsMachines = new List<IEnemyHelper>();

        Collider[] colliders = Physics.OverlapSphere(center, radius);

        foreach (Collider col in colliders)
        {
            UnitStateMachine machine = col.gameObject.GetComponent<UnitStateMachine>();

            if (machine != null) {
//                Debug.Log("found unit of " + machine.Helper.MyArmy);
                if (machine.EnemyHelper.MyArmy != MyArmy && machine.EnemyHelper.IsAlife())  {
//                    Debug.Log("Adding to list unit of " + machine.Helper.MyArmy);
                    unitsMachines.Add(machine.EnemyHelper);
                }
            }
//            Debug.Log("-------------");

        } // foreach collider //

        return unitsMachines;
    } // CheckEnemies() //




} // End of class //

