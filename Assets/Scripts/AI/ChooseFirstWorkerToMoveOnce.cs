using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Choose First Worker To Move Once")]
public class ChooseFirstWorkerToMoveOnce : Condition
{
    private TeamOrchestrator m_teamOrchestrator = null;

    public override void OnEnter()
    {
        m_teamOrchestrator = TeamOrchestrator._Instance;
    }

    public override bool Check()
    {
        if (!m_teamOrchestrator.FirstAssigned)
        {
            m_teamOrchestrator.FirstAssigned = true;
            return true;
        }

        return false;        
    }
}
