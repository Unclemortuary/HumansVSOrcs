using UnityEngine;

public class AIUpdaterComponent : MonoBehaviour {

    AIController controller;

    public void SetAIController(AIController controller) {
        this.controller = controller;
    }

    void Update() {
        if (GameManager.Instance.GameIsStarted) {
            controller.Update();
        }
    }

}