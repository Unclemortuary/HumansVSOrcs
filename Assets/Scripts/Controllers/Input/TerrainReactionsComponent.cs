using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainReactionsComponent : MonoBehaviour {


    private ArmyDispatcher dispatcher;

    public void SetDispatcher(ArmyDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    void Start() {
        GameManager.Instance.HereIsTerrain(this);

//        InitializeEventTrigger();

        Debug.Log(">>> Terrain Reactions Start exit OK");
    }






    public void InitializeEventTrigger() {

        EventTrigger eventTrigger = GetComponent<EventTrigger>( );

        if (eventTrigger == null) {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry( );

        entry.eventID = EventTriggerType.PointerDown;

        entry.callback.AddListener( ( data ) => { IAmClicked( (PointerEventData)data ); } );

        eventTrigger.triggers.Add( entry );


    } // InitializeEventTrigger() //


    public void IAmClicked(PointerEventData data) {


        if (data.button == PointerEventData.InputButton.Right)
        {
            dispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.terrainIsRightClicked, data.pointerPressRaycast.worldPosition);
        }
        else if (data.button == PointerEventData.InputButton.Left)
        {
            dispatcher.TriggerCommand<Vector3>(ArmyMessageTypes.terrainIsLeftClicked, data.pointerPressRaycast.worldPosition);
//            dispatcher.TriggerCommand<Vector2>(ArmyMessageTypes.terrainIsLeftClicked, data.pressPosition);
        }

    }

}