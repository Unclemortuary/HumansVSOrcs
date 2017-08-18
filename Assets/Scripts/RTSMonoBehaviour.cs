using CommandsDispatcher;
using UnityEngine;

public class RTSMonoBehaviour : MonoBehaviour {

    ArmyManager armyManager;
//    CommandDispatcher<> armyDispatcher;

    void Start() {
        GameManager.Instance.HereIAm(this);
    }


}
