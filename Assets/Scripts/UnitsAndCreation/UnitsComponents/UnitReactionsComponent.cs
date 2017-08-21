using UnityEngine;
using UnityEngine.EventSystems;

public class UnitReactionsComponent : RTSMonoBehaviour {

    private AbstractGameUnit thisUnit;

    public void SetGameUnit(AbstractGameUnit unit) {
        thisUnit = unit;
    }

    void Start() {

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

//        entry.callback.AddListener( ( data ) => { OnPointerDown( (PointerEventData)data ); } );
        entry.callback.AddListener( ( data ) => { IAmClicked( (PointerEventData)data ); } );

        eventTrigger.triggers.Add( entry );


    } // InitializeEventTrigger() //



    private void SubscribeOnDispatcherMessages() {

        // Подписка на события здесь

    }

////////////////////////////////////////////////////////////////////////////////

    public void IAmClicked(PointerEventData data) {
        // Отправка сообщения
        // unit -- в качестве аргумента?
        // или все-таки unitID?

        if (data.button == PointerEventData.InputButton.Left)
        {
            armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitLeftClicked, thisUnit);
        }
        else if (data.button == PointerEventData.InputButton.Right)
        {
            armyManager.Dispatcher.TriggerCommand<AbstractGameUnit>(ArmyMessageTypes.unitRightClicked, thisUnit);
        }

    }

    public void IAmDead() {
        // Послать сообщение о том, что юнит погиб //
    }

    public void OnPointerDown( PointerEventData data )
    {
        print( "OnPointerDown called." );
    }


////////////////////////////////////////////////////////////////////////////////////

    void Update() {

        // Update Healthbar //

    }


}