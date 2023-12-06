using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomState : BrainState
{
    List<Worker> m_leaders = new List<Worker>();


    public override void OnEnter()
    {
        Debug.Log("Entering DoomState");

        PairUpWorkers();
    }

    private void PairUpWorkers()
    {
        int index = 0;
        Worker lastLeader = null;
        foreach (var worker in TeamOrchestrator._Instance.WorkersList)
        {
            worker.SetIsInDoomStateBool(true);

            if (index % 2 == 0)
            {
                m_leaders.Add(worker);
                worker.SetAsLeader(true);
                lastLeader = worker;
            }
            else
            {
                worker.AssignLeader(lastLeader);
                lastLeader.AssignFollower(worker.transform);
            }
            index++;
        }
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
