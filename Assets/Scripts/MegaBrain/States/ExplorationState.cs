using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationState : BrainState
{
    private const float DENSITY_AREA_RADIUS = 5.0f;


    public override void OnEnter()
    {
        Debug.Log("Entering ExplorationState");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        Debug.Log("Exiting ExplorationState");

        /*
        CalculateMinimumCollectiblesNeededPerCamp()

        * Could all be in ExploitationState *

        Team.Orchestrator.EvaluateStrategy()
            Expand ?
            Collect ?
            Do nothing ?

        Team.Orchestrator.OrganizeCollectibles()
            Sort Collectibles
            Organize in arrays (subgroups)
            
        Team.Orchestrator.DelegateWorkers()
            1 to camp
            others to assigned collectible

        Team.Orchestrator.SpawnWorkers() ?

         */

        ////////////////

        //m_stateMachine.SetCollectiblesDensity(CalculateCollectiblesDensity());
        //Debug.Log("density: " + m_stateMachine.m_collectiblesDensity);
        //
        //m_stateMachine.SetAverageCollectiblesDistance(CalculateAverageCollectiblesDistance());
        //Debug.Log("av. dist.: " + m_stateMachine.m_averageCollectiblesDistance);
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
