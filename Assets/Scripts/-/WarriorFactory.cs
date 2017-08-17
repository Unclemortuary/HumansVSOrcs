using UnityEngine;

public class WarriorFactory : AbstractWarriorFactory {


    public WarriorFactory(AbstractGameUnitCreator archersCreator,
                            AbstractGameUnitCreator swordsmenCreator,
                            AbstractGameUnitCreator horsemenCreator) {

        this.archersCreator = archersCreator;
        this.swordsmenCreator = swordsmenCreator;
        this.horsemenCreator = horsemenCreator;
    }

}