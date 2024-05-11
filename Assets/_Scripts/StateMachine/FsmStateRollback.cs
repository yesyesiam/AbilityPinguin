using FSM._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmStateRollback : FsmState
{
    public FsmStateRollback(Fsm fsm) : base(fsm)
    {
    }
    public override void Enter()
    {
        Debug.Log("Rollback Enter");
    }

    public override void Exit()
    {
        Debug.Log("Rollback Exit");
    }
}
