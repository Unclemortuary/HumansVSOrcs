using UnityEngine;

public class DestroyTimer {

    public DestroyTimer(GameObject obj, float time) {
        new Timer(obj, delegate  {
            GameObject.Destroy(obj);
        }, time);
    }
}