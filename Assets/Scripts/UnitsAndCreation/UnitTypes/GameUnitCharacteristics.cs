using UnityEngine;

[System.Serializable]
public class GameUnitCharacteristics {

    [SerializeField]
    private GameObject deadAvatar;
    public GameObject DeadAvatar {
        get {
            return deadAvatar;
        }
    }

    [SerializeField]
    private float maxAttackDistance;
    public float MaxAttackDistance {
        get {
            return maxAttackDistance;
        }
    }

    [SerializeField]
    private float maxViewDistance;
    public float MaxViewDistance {
        get {
            return maxViewDistance;
        }
    }

    [SerializeField]
    private float attackPhisDamage;
    public float AttackPhisDamage {
        get {
            return attackPhisDamage;
        }
    }

    [SerializeField]
    private float attackCooldownTime;
    public float AttackCooldownTime {
        get {
            return attackCooldownTime;
        }
    }




    //------------------------------------------

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
    private float maxMovingSpeed;
    public float MaxMovingSpeed {
        get {
            return maxMovingSpeed;
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


    [SerializeField]
    private float dropGold;
    public float DropGold {
        get {
            return dropGold;
        }
    }

    public GameUnitCharacteristics(GameObject deadAvatar, float attackDistance, float viewDistance, float damage, float cooldown,
                float hp, float mp,
                float speed, float defence, float drop /*, float time*/) {

        this.deadAvatar = deadAvatar;

        this.maxAttackDistance = attackDistance;
        this.maxViewDistance = viewDistance;
        this.attackPhisDamage = damage;
        this.attackCooldownTime = cooldown;

        this.maxHP = hp;
        this.maxMP = mp;
        this.maxMovingSpeed = speed;
        this.defence = defence;

        this.dropGold = drop;
//        this.timeToCreate = time;
    }

    public GameUnitCharacteristics CreateCopy() {
        return new GameUnitCharacteristics(deadAvatar, maxAttackDistance, maxViewDistance, attackPhisDamage, attackCooldownTime,
                maxHP, maxMP, maxMovingSpeed, defence, dropGold/*, timeToCreate*/);
    }


}