using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationState : BrainState
{
    private const float DENSITY_AREA_RADIUS = 5.0f;

    private bool m_LaunchingExplorationPhase = false;

    public override void OnEnter()
    {
        Debug.Log("Entering ExplorationState");

        //SetWorkersExplorationBool(true);

        for (int i = 0; i < 5; i++)
        {
            TeamOrchestrator._Instance.SpawnWorker();
        }
    }

    private void SetWorkersExplorationBool(bool value)
    {
        foreach (var worker in TeamOrchestrator._Instance.WorkersList)
        {
            worker.SetIsInExplorationPhaseBool(value);
        }
    }

    public override void OnFixedUpdate()
    {
        if (m_LaunchingExplorationPhase == false)
        {
            SetWorkersExplorationBool(true);
            m_LaunchingExplorationPhase = true;
        }
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        Debug.Log("Exiting ExplorationState");

        SetWorkersExplorationBool(false);
    }

    /*private float CalculateCollectiblesDensity()
    {
        float collectiblesAmount = 0;
        
        foreach (var collectible in TeamOrchestrator._Instance.KnownCollectibles)
        {
            collectiblesAmount++;
        }

        return (float)(collectiblesAmount / (Math.PI * DENSITY_AREA_RADIUS * DENSITY_AREA_RADIUS));
    }

    private float CalculateAverageCollectiblesDistance()
    {
        double poisson = DENSITY_AREA_RADIUS / (2 * Math.Sqrt(m_stateMachine.m_collectiblesDensity));

        return (float)poisson;
    }*/

    public override bool CanEnter(IState currentState)
    {
        return false;
    }
    public override bool CanExit()
    {
        return true;
    }
}
