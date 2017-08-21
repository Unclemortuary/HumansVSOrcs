using CommandDispatcher;
using CommandsDispatcher;
using UnityEngine;

public class RTSMonoBehaviour : MonoBehaviour {

    protected ArmyManager armyManager;

    public void SetArmyManager(ArmyManager armyManager) {
        this.armyManager = armyManager;
    }


    void Start() {
        GameManager.Instance.HereIAm(this);
    }


}
