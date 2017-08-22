using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitReactionsComponent : RTSMonoBehaviour {

    // Selection light circle //
    private GameObject projector;

    private AbstractGameUnit thisUnit;

    public void SetGameUnit(AbstractGameUnit unit) {
        thisUnit = unit;
    }

    private NavMeshAgent agent;
    private float stoppingDistance  = 3;

    public void SetNavmeshAgent(NavMeshAgent agent) {
        this.agent = agent;
        agent.stoppingDistance = stoppingDistance;
    }


    void Start() {
//        base.Start();

        if (gameObject.transform.Find("Projector") == null) {
            print("Can't find a Projector");
        }

        projector = gameObject.transform.Find("Projector").gameObject;

        InitializeEventTrigger();

        SubscribeOnDispatcherMessages();
    }


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
                    agent.destination = pos;
                },
            thisUnit.ID
        );

        armyManager.Dispatcher.StartListening(ArmyMessageTypes.unitCommandStop,
                () => {
                    agent.ResetPath();
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

//        if (data == null) {
//            print("pointer event data is null");
//        }
//        if (thisUnit == null) {
//            print("thisUnit is null");
//        }
//        if (armyManager == null) {
//            print("Armymanager is null");
//        }
//        if (armyManager.Dispatcher == null) {
//            print("army dispatcher is null");
//        }
//
//        print("Sending message IAmClicked");

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