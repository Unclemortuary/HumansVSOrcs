using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWarriorFactory  {

    /**
        !!! Initialization of Creators is needed !!!
     */

    //public abstract void InitializeCreators();

    protected AbstractGameUnitCreator archersCreator = null;
    protected AbstractGameUnitCreator swordsmenCreator = null;
    protected AbstractGameUnitCreator horsemenCreator = null;



/////////////////////////////////////////////////////////////////////////

    protected AbstractGameUnitCreator ArchersCreator {
        get {
            return archersCreator;
        }
    }

    protected AbstractGameUnitCreator SwordsmenCreator {
        get {
            return swordsmenCreator;
        }
    }

    protected AbstractGameUnitCreator HorsemenCreator {
        get {
            return horsemenCreator;
        }
    }



    public AbstractGameUnit CreateArcher(Vector3 position) {
        return ArchersCreator.CreateGameUnit(position);
    }

    public AbstractGameUnit CreateSwordsman(Vector3 position) {
        return SwordsmenCreator.CreateGameUnit(position);
    }

    public AbstractGameUnit CreateHorseman(Vector3 position) {
        return HorsemenCreator.CreateGameUnit(position);
    }

}
