using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aurora_Idle_State : AStateBehaviour
{
    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateEnd()
    {
        throw new System.NotImplementedException();
    }

    public override int StateTransitionCondition()
    {
        return (int)EAuroraStates.Invalid;
    }
}
