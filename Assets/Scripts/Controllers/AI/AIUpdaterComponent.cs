using UnityEngine;

public class AIUpdaterComponent : MonoBehaviour {

    AIController controller;

    private float period = 5f;
    private float timer;

    public void SetAIController(AIController controller) {
        this.controller = controller;

        timer = period;
    }

    public void SetTimePeriod(float period) {
        this.period = period;
    }

    void Update() {

        timer -= Time.deltaTime;

        if (GameManager.Instance.GameIsStarted && timer <= 0) {
            controller.Update();

            timer = period;
        }
    }

}