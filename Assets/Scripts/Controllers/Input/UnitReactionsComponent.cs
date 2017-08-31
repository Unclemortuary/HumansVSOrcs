using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitReactionsComponent : RTSMonoBehaviour {

    // Selection light circle //
    private GameObject projector;


    private AbstractGameUnit thisUnit;
    public AbstractGameUnit ThisUnit {
        get {
            return thisUnit;
        }
    }

    public void SetGameUnit(AbstractGameUnit unit) {
        thisUnit = unit;

// -----------------
        projector = gameObject.transform.Find("Projector").gameObject;

        InitializeEventTrigger();

        SubscribeOnDispatcherMessages();

    }

    private NavMeshAgent agent;
    public NavMeshAgent Agent {
        get {
            return agent;
        }
    }
    private float stoppingDistance  = 3;

    public void SetNavmeshAgent(NavMeshAgent agent) {
        this.agent = agent;
        agent.stoppingDistance = stoppingDistance;
    }


    void Start() {

//        projector = gameObject.transform.Find("Projector").gameObject;
//
//        InitializeEventTrigger();
//
//        SubscribeOnDispatcherMessages();
//

    }

//    private void InitializeBuilderComponent() {
//        BuilderReactionsComponent builder = GetComponent<BuilderReactionsComponent>();
//
//        if (builder != null) {
//            builder.Initialize(thisUnit);
//        }
//    }


    private void InitializeEventTrigger() {
//        gameObject.AddComponent<EventTriggerTest>();

        EventTrigger eventTrigger = GetComponent<EventTrigger>( );

        if (eventTrigger == null) {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry( );

        entry.eventID = EventTriggerType.PointerDown;

        entry.callback.AddListener( ( data ) => { IAmClicked( (PointerEventData)data ); } );

        eventTrigger.triggers.Add( entry );


    } // InitializeEventTrigger() //



    private void SubscribeOnDispatcherMessages() {

        // Подписка на события здесь
        armyManager.Dispatcher.StartListening<bool>(ArmyMessageTypes.unitCommandTurnSelection,
                (bool on) => {
                    TurnSelection(on);
                },
            thisUnit.ID
        );


        armyManager.Dispatcher.StartListening<Vector3>(ArmyMessageTypes.unitCommandGoToPosition,
                (Vector3 pos) => {
                    if (ThisUnit.IsActive) {
                        agent.destination = pos;
                    }
                },
            thisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
                () => {
                    if (ThisUnit.IsActive) {
                        agent.ResetPath();
                    }
                },
            thisUnit.ID
        );



        // ############################################3333
//        unitCommandFollowUnit,
//        unitCommandStop,


    }

    private void TurnSelection(bool on) {
        projector.SetActive(on);
    }

////////////////////////////////////////////////////////////////////////////////


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

    public void IAmDead() {
        // Send message to controller //
        armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead, thisUnit);
    }





////////////////////////////////////////////////////////////////////////////////////

    void Update() {

        // Update Healthbar //


        // is it dead?
        // IAmDead();


        // Set target for NavMesh //
        // If following a unit, destination redefinition is needed //




    } // Update() //



} // End of class //