

public class BrainState : IState
{
    protected MegaBrainSM m_stateMachine;
    public void OnStart(MegaBrainSM controller)
    {
        m_stateMachine = controller;
    }

    public void OnStart()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual bool CanEnter(IState currentState)
    {
        return true;
    }
    public virtual bool CanExit()
    {
        return true;
    }
}
