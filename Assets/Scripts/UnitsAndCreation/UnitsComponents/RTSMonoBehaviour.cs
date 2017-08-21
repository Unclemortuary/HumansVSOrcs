using CommandDispatcher;
using CommandsDispatcher;
using UnityEngine;

public class RTSMonoBehaviour : MonoBehaviour {

    protected ArmyManager armyManager;

    public void SetArmyManager(ArmyManager armyManager) {
        this.armyManager = armyManager;
    }


    void Awake() {
//        print("rtsmonobeh Awake called");
        GameManager.Instance.HereIAm(this);
    }
}
