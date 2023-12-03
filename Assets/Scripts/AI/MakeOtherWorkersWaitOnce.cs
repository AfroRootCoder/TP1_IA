using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Make Other Workers Wait Once")]
public class MakeOtherWorkersWaitOnce : Condition
{
    private TeamOrchestrator m_teamOrchestrator = null;

    public override void OnEnter()
    {
        m_teamOrchestrator = TeamOrchestrator._Instance;
        m_teamOrchestrator.NumberOfWorkers++;
    }

    public override bool Check()
    {
        if(m_teamOrchestrator.NumberOfWorkers < m_teamOrchestrator.WorkersList.Count)
        {
            return true;
        }

        //if (!m_teamOrchestrator.MovingFirst)
        //{
        //    m_teamOrchestrator.MovingFirst = true;
        //    return true;
        //}

        return false;
    }

    
}
