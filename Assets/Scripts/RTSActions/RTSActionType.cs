
[System.Serializable]
public enum RTSActionType {
	NULL,
    moveTo,
	stop,
	attack,
    holdPosition,
    patrol,


    buildFarm,
	buildGeneralHouse,
    buildBarrack,
    buildForge,
    buildQuarry,
    buildSawmill,
    buildSimpleHouse,
    buildWatchTower,

    workersBuildFarm,
    workersBuildGeneralHouse,
    workersBuildBarrack,
    workersBuildForge,
    workersBuildQuarry,
    workersBuildSawmill,
    workersBuildSimpleHouse,
    workersBuildWatchTower,

    createArcher,
    createSwordsman,
    createHorseman,
    createWorker,

	sell,
    specialHeal,
	specialFireStorm
}