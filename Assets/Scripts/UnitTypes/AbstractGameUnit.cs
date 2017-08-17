using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGameUnit {

    public abstract int ID { get; set;}
    public abstract string Description { get; set; }

    public abstract float CurrentHP { get; }
    public abstract float CurrentMP { get; }

    public abstract GameObject Avatar { get; }


    public abstract GameUnitCharacteristics Characteristics { get; }


    /*    public float MaxAttackDistance { get; }
        public float MaxSpeed { get; }
        public float MaxHP { get; }
        public float MaxMP { get; }
        public float Defence { get; }
    */

}
