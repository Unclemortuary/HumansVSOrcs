using UnityEngine;

public class CameraHolderComponent : MonoBehaviour {

    void Start() {
        GameManager.Instance.HereIsCameraHolder(this);
    }
}