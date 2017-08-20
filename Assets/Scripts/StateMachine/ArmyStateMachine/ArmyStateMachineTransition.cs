using System;

public class ArmyStateMachineTransition : StateMachineTransition<ArmySMStateType>{

    public ArmyStateMachineTransition(ArmySMStateType from, ArmySMStateType to,
            Action<ArmySMStateType, ArmySMStateType> action) :     base(from, to, action) {
    }
}