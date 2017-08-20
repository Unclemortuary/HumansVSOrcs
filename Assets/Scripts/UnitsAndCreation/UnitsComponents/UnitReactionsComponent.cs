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
        gameObject.AddComponent<EventTriggerTest>();

//        EventTrigger eventTrigger = GetComponent<EventTrigger>( );
//
//        if (eventTrigger == null) {
//            eventTrigger = gameObject.AddComponent<EventTrigger>();
//        }
//
//        EventTrigger.Entry entry = new EventTrigger.Entry( );
//
//        entry.eventID = EventTriggerType.PointerDown;
//
//        entry.callback.AddListener( ( data ) => { OnPointerDown( (PointerEventData)data ); } );
//        entry.callback.AddListener( ( data ) => { OnPointerDownDelegate( (PointerEventData)data ); } );
//
//        eventTrigger.triggers.Add( entry );
//

    }



    private void SubscribeOnDispatcherMessages() {

        // Подписка на события здесь

    }

////////////////////////////////////////////////////////////////////////////////

    public void IAmClicked() {
        // Отправка сообщения
        // unit -- в качестве аргумента?
        // или все-таки unitID?
    }

    public void IAmDead() {
        // Послать сообщение о том, что юнит погиб //
    }

    public void OnPointerDown( PointerEventData data )
    {
        print( "OnPointerDown called." );
    }

    public void OnPointerDownDelegate( PointerEventData data )
    {
        Debug.Log( "OnPointerDownDelegate called." );
    }

////////////////////////////////////////////////////////////////////////////////////

    void Update() {

        // Update Healthbar //

    }


}