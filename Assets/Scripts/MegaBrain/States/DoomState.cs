using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoomState : BrainState
{



    public override void OnEnter()
    {
        Debug.Log("Entering DoomState");
        PairWorkers();
        FindClosestFollowerForEachLeader();
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

    private void PairWorkers()
    {
        for (int i = 0; i < TeamOrchestrator._Instance.WorkersList.Count; i++)
        {
            if (i % 2 == 0)
            {
                TeamOrchestrator._Instance.WorkersList[i].SetLeadershipStatus(true);
            }
        }
    }

    private void FindClosestFollowerForEachLeader()
    {
        foreach (var worker in TeamOrchestrator._Instance.WorkersList)
        {
            float distanceBetweenLeaderAndFollower = float.MaxValue;
            if (worker.GetLeadershipStatus() == false)
            {
                continue;
            }
            else
            {
                foreach (var worker2 in TeamOrchestrator._Instance.WorkersList)
                {
                    if (worker2.GetLeadershipStatus() == true)
                    {
                        continue;
                    }
                    else
                    {
                        float distance = Vector2.Distance(worker.GetPosition(), worker2.GetPosition());
                        if (distance < distanceBetweenLeaderAndFollower)
                        {
                            distanceBetweenLeaderAndFollower = distance;
                        }
                    }
                }
            }
        }
    }
}
