using UnityEngine;

public class ExplorationState : BrainState
{
    private const float DENSITY_AREA_RADIUS = 5.0f;

    private bool m_LaunchingExplorationPhase = false;

    public override void OnEnter()
    {
        Debug.Log("Entering ExplorationState");

        TeamOrchestrator._Instance.SpawnStartingWorkers();
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

    public override bool CanEnter(IState currentState)
    {
        return false;
    }
    public override bool CanExit()
    {
        return true;
    }
}
