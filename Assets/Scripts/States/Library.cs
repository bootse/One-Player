using UnityEngine;
using System.Collections;
using System;

public class Library : State
{
    public Library(StateMachine state) : base(state)
    {

    }

    public override void AfterUpdate()
    {
        throw new NotImplementedException("Function used for Player");
    }

    public override void EnterState()
    {
        throw new NotImplementedException();
    }

    public override void LeaveState()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }
}
