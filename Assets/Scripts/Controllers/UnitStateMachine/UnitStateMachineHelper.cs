using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitStateMachineHelper {

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
        projector.SetActive(on);
    }


    ///////////////////////////////////////////////////////////////
    // --------------------------------------------------------- //
    // ********************************************************* //
    ///////////////////////////////////////////////////////////////

    // Building ///////////////////////////////////////////////////



    private float workingRadius = 225f; //15;
    private float workingRadiusDelta = 3f;

    private AbstractGameUnit targetBuilding;


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
            return targetBuilding;
        }
        set {
            targetBuilding = value;
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







} // End of class //

