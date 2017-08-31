public enum ArmyMessageTypes {

        // Unit -> Controller //
        unitCryRightClicked,
        unitCryLeftClicked,
        unitCryIsDead,

        // Terrain -> Controller //
        terrainIsLeftClicked,
        terrainIsRightClicked,

        // Controller -> StateMachine //
        addUnitsToSelection,
        excludeUnitFromSelection,
        deselectUnits,
        invokeRTSAction,

        addBuildingToSelection,

        // Controller -> StateMachine
        testWaitingForTarget,
        setTargetPoint,
        setTargetUnit,

        // Controller -> StateMachine
        stopMachine,


        // StateMachine or Controller -> unit
        unitCommandTurnSelection,
        unitCommandGoToPosition,
        unitCommandFollowUnit,
        unitCommandStop,
        unitCommandBuild,
        unitCommandSetWorkDuration,

        // StateMachine -> HUD
        selectionChanged,
        refreshSelection,

    ////////////
        testUnitInSelection,


}