using UnityEngine;

public class GameUnitID : RTSMonoBehaviour {

    [SerializeField]
    private int personalID;
    public int PersonalID {
        get {
            return personalID;
        }
        set {
            personalID = value;
        }
    }

    [SerializeField]
    private Identification.Army army;
    public Identification.Army Army {
        get {
            return army;
        }
        set {
            army = value;
        }
    }

}