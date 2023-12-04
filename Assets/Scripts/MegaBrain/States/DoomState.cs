using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomState : BrainState
{



    public override void OnEnter()
    {
        Debug.Log("Entering DoomState");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        Debug.Log("Exiting DoomState");
    }

    public override bool CanEnter(IState currentState)
    {
        return TeamOrchestrator._Instance.GetRemainingTime() < 100.0f;
    }
    public override bool CanExit()
    {
        return false;
        //To check
    }
}
