using UnityEngine;

public class BuildingProgressComponent : RTSMonoBehaviour {

    [SerializeField]
    private bool finished = false;
    public bool Finished {
        get {
            return finished;
        }
    }

    [SerializeField]
    private ITaskerHelper helper;
    public ITaskerHelper Helper {
        get {
            return helper;
        }
        set {
            helper = value;
        }
    }


    void Update() {
        if (!finished && helper.TaskRemaintinTime <= 0) {
            finished = true;
            helper = null;
        }
    }

}