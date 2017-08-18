using CommandDispatcher;
using CommandsDispatcher;
using UnityEngine;

public class RTSMonoBehaviour : MonoBehaviour {

    ArmyManager armyManager;
    ArmyDispatcher armyDispatcher;

    public void SetArmyManager(ArmyManager armyManager) {
        this.armyManager = armyManager;
    }

    public void SetArmyDispatcher(ArmyDispatcher armyDispatcher) {
        this.armyDispatcher = armyDispatcher;
    }

    void Start() {
        GameManager.Instance.HereIAm(this);
    }


}
