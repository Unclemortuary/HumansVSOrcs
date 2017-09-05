using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractGameUnit {

    private bool isActive = true;
    public bool IsActive {
        get {
            return isActive;
        }
        set {
            isActive = value;
        }
    }

    public abstract float Radius { get; }


    public abstract int ID { get; set;}
    public abstract string Description { get; set; }

    public abstract float CurrentHP { get; }
    public abstract float CurrentMP { get; }
    public abstract void ChangeHP(float value);
    public abstract void ChangeMP(float value);

    public abstract GameObject Avatar { get; }



    public abstract void Nullify();

    public abstract bool IsDead();

    public abstract GameUnitCharacteristics Characteristics { get; }

    public abstract List<RTSActionType> ActionsList { get; }

}
