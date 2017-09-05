
[System.Serializable]
public static class Identification {

    [System.Serializable]
    public enum Army { Humans, Orcs, Neutrals };

    [System.Serializable]
    public enum UnitType {
        Archer, Swordsman, Horseman,
        GeneralHouse, Barrack, Farm, Forge, Quarry, Sawmill, SimpleHouse, WatchTower,
        Worker,
        FlyingWarrior,
        Mine,

        // Neutrals //
        Troll,
        Zombie,
        Barbarian,
        Ghost,
        Skeleton,
        SkeletonHard,

        Archer2, Archer3,
        Swordsman2, Swordsman3,
    };



}