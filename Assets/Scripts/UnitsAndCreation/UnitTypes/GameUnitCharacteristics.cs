using UnityEngine;

[System.Serializable]
public class GameUnitCharacteristics {

    [SerializeField]
    private float maxAttackDistance;
    public float MaxAttackDistance {
        get {
            return maxAttackDistance;
        }
    }

    [SerializeField]
    private float maxHP;
    public float MaxHP {
        get {
            return maxHP;
        }
        set {
            maxHP = value;
        }
    }



    [SerializeField]
    private float maxMP;
    public float MaxMP {
        get {
            return maxMP;
        }
    }


    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed {
        get {
            return maxSpeed;
        }
    }

    [SerializeField]
    private float defence;
    public float Defence {
        get {
            return defence;
        }
    }

//    [SerializeField]
//    private float timeToCreate;
//    public float TimeToCreate {
//        get {
//            return timeToCreate;
//        }
//    }

    public GameUnitCharacteristics(float attackDistance, float hp, float mp,
                float speed, float defence/*, float time*/) {
        this.maxAttackDistance = attackDistance;
        this.maxHP = hp;
        this.maxMP = mp;
        this.maxSpeed = speed;
        this.defence = defence;
//        this.timeToCreate = time;
    }

    public GameUnitCharacteristics CreateCopy() {
        return new GameUnitCharacteristics(maxAttackDistance, maxHP, maxMP, maxSpeed, defence/*, timeToCreate*/);
    }


}