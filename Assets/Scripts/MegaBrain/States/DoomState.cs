using MBT;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoomState : BrainState
{
    public override void OnEnter()
    {
        Debug.Log("Entering DoomState");
        UnassignWorkers();
        PairWorkers();
        FindClosestFollowerForEachLeader();
        SetDoomStateInWorkers();
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
        foreach (var leader in TeamOrchestrator._Instance.WorkersList)
        {
            float distanceBetweenLeaderAndFollower = float.MaxValue;
            if (!leader.GetLeadershipStatus())
            {
                continue;
            }
            else
            {
                int closestFollowerIndex = 0;

                for (int i = 0; i < TeamOrchestrator._Instance.WorkersList.Count; i++)
                {

                    if (TeamOrchestrator._Instance.WorkersList[i].GetLeadershipStatus()|| 
                        TeamOrchestrator._Instance.WorkersList[i].GetAssignationStatus())
                    {
                        continue;
                    }
                    else
                    {
                        float distance = Vector2.Distance(leader.GetPosition(), TeamOrchestrator._Instance.WorkersList[i].GetPosition());
                        if (distance < distanceBetweenLeaderAndFollower)
                        {
                            distanceBetweenLeaderAndFollower = distance;
                            closestFollowerIndex = i;
                        }
                        if (i == TeamOrchestrator._Instance.WorkersList.Count - 1)
                        {
                            leader.SetFollower(closestFollowerIndex);
                            leader.SetIsAssignedBool(true);
                            TeamOrchestrator._Instance.WorkersList[closestFollowerIndex].SetIsAssignedBool(true);
                        }
                    }
                }
            }
        }
    }

    private void SetDoomStateInWorkers()
    {
        foreach(var worker in TeamOrchestrator._Instance.WorkersList)
        {
            worker.SetIsInDoomPhaseBool(true);
        }
    }

    private void UnassignWorkers()
    {
        foreach(var worker in TeamOrchestrator._Instance.WorkersList)
        {
            worker.SetIsAssignedBool(false);
        }
    }
}
