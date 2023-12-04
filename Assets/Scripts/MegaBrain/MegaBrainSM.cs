using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBrainSM : BaseStateMachine<BrainState>
{
    public float m_stateTimer { get; private set; } = 0.0f;
    public float m_collectiblesDensity { get; private set; } = 0.0f;
    public float m_averageCollectiblesDistance { get; private set; } = 0.0f;

    [field: SerializeField] public float ExplorationStateDuration { get; private set; }


    ///////

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<BrainState>();

        m_possibleStates.Add(new ExplorationState());
        m_possibleStates.Add(new ExploitationState());
        m_possibleStates.Add(new DoomState());
    }

    protected override void Start()
    {
        foreach (BrainState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_currentState is ExplorationState)
        {
            m_stateTimer += Time.fixedDeltaTime;
        }

    }

    public void SetCollectiblesDensity(float density)
    {
        m_collectiblesDensity = density;
    }

    public void SetAverageCollectiblesDistance(float distance)
    {
        m_averageCollectiblesDistance = distance;
    }
}
